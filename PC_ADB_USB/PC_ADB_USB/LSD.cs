﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PC_ADB_USB
{
    public struct coord
    {
        public int x, y;
    };

    public class image<T>
    {
        public T[,] data;
        public uint width, height;
        public static image<T> newImage(uint x, uint y)
        {
            image<T> image = new image<T>();
            image.data = new T[x, y];
            image.width = x;
            image.height = y;
            return image;
        }
    };

    class LSD
    {
        const double M_LN10=2.30258509299404568402;
        const double M_PI  =Math.PI;        //3.14159265358979323846
        public const double NOTDEF=-1024.0;
        const double M_3_2_PI=3*Math.PI/2;  //4.71238898038
        const double M_2__PI=2*Math.PI;     //6.28318530718


        
        public class doubleTupleList
        {
            public uint dim;
            public List<double[]> values=new List<double[]>();  
            static public doubleTupleList newTupleList(uint dim)
            {
                doubleTupleList t=new doubleTupleList();
                t.dim = dim;
                return t;
            }
            public int size { get { return values.Count; } }
            public bool Add7(double v0,double v1, double v2, double v3, double v4, double v5, double v6)
            {
                if (dim != 7) return false;
                values.Add(new double[7] { v0,v1,v2,v3,v4,v5,v6} );
                return true;
            }
        };



        static void error(string msg)
        {
            MessageBox.Show("LSD Error:"+msg);
            Application.Exit();
        }

        static bool double_equal(double a, double b)
        {
            const double RELATIVE_ERROR_FACTOR = 100.0;
            double abs_diff,aa,bb,abs_max;
            if( a == b ) return true;
            abs_diff = Math.Abs(a-b);
            aa = Math.Abs(a);
            bb = Math.Abs(b);
            abs_max = aa > bb ? aa : bb;
            if( abs_max < double.MinValue ) abs_max = double.MinValue;
            return (abs_diff / abs_max) <= (RELATIVE_ERROR_FACTOR * double.Epsilon);
        }

        static double dist(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt( (x2-x1)*(x2-x1) + (y2-y1)*(y2-y1) );
        }

        static void gaussian_kernel(double[] kernel, double sigma, double mean)
        {
            double sum = 0.0;
            double val;
            if( sigma <= 0.0 ) error("gaussian_kernel: 'sigma' must be positive.");
            // compute Gaussian kernel
            uint dim=(uint)kernel.Length;
            for(uint i=0;i<dim;i++)
            {
                val = ( (double) i - mean ) / sigma;
                kernel[i] = Math.Exp( -0.5 * val * val );
                sum += kernel[i];
            }
            // normalization 
            if( sum >= 0.0 ) for(uint i=0;i<dim;i++) kernel[i] /= sum;
        }

        /** Scale the input image 'in' by a factor 'scale' by Gaussian sub-sampling.

        For example, scale=0.8 will give a result at 80% of the original size.

        The image is convolved with a Gaussian kernel
        @f[
            G(x,y) = \frac{1}{2\pi\sigma^2} e^{-\frac{x^2+y^2}{2\sigma^2}}
        @f]
        before the sub-sampling to prevent aliasing.

        The standard deviation sigma given by:
        -  sigma = sigma_scale / scale,   if scale <  1.0
        -  sigma = sigma_scale,           if scale >= 1.0

        To be able to sub-sample at non-integer steps, some interpolation
        is needed. In this implementation, the interpolation is done by
        the Gaussian kernel, so both operations (filtering and sampling)
        are done at the same time. The Gaussian kernel is computed
        centered on the coordinates of the required sample. In this way,
        when applied, it gives directly the result of convolving the image
        with the kernel and interpolated to that particular position.

        A fast algorithm is done using the separability of the Gaussian
        kernel. Applying the 2D Gaussian kernel is equivalent to applying
        first a horizontal 1D Gaussian kernel and then a vertical 1D
        Gaussian kernel (or the other way round). The reason is that
        @f[
            G(x,y) = G(x) * G(y)
        @f]
        where
        @f[
            G(x) = \frac{1}{\sqrt{2\pi}\sigma} e^{-\frac{x^2}{2\sigma^2}}.
        @f]
        The algorithm first applies a combined Gaussian kernel and sampling
        in the x axis, and then the combined Gaussian kernel and sampling
        in the y axis.
        */
        public static image<double> gaussian_sampler( image<double> inImage, double scale,double sigma_scale )
        {
            image<double> aux,outImage;
            
            uint N,M,h,n,x,y,i;
            int xc,yc,j,double_x_size,double_y_size;
            double sigma,xx,yy,sum,prec;
            /* check parameters */
            if( inImage == null || inImage.data == null || inImage.width == 0 || inImage.height == 0 )
                error("gaussian_sampler: invalid image.");
            if( scale <= 0.0 )       error("gaussian_sampler: 'scale' must be positive.");
            if( sigma_scale <= 0.0 ) error("gaussian_sampler: 'sigma_scale' must be positive.");
            if( inImage.width * scale > (double) uint.MaxValue || inImage.height * scale > (double) uint.MaxValue )
                error("gaussian_sampler: the output image size exceeds the handled size.");

            N = (uint) Math.Ceiling( inImage.width * scale );
            M = (uint) Math.Ceiling( inImage.height * scale );
            aux      = image<double>.newImage(N,inImage.height);
            outImage = image<double>.newImage(N,M);

            // sigma, kernel size and memory for the kernel 
            sigma = scale < 1.0 ? sigma_scale / scale : sigma_scale;
            // The size of the kernel is selected to guarantee that the
            // the first discarded term is at least 10^prec times smaller
            // than the central value. For that, h should be larger than x, with
            // e^(-x^2/2sigma^2) = 1/10^prec.
            // Then x = sigma * sqrt( 2 * prec * ln(10) ).
            prec = 3.0;
            h = (uint) Math.Ceiling( sigma * Math.Sqrt( 2.0 * prec * M_LN10) );
            n = 1+2*h;  // kernel size 
            double[] kernel = new double[n];
            uint dim=n;
            /* auxiliary double image size variables */
            double_x_size = (int) (2 * inImage.width);
            double_y_size = (int) (2 * inImage.height);

            /* First subsampling: x axis */
            for(x=0;x<aux.width;x++)
            {
                // x   is the coordinate in the new image.
                // xx  is the corresponding x-value in the original size image.
                // xc  is the integer value, the pixel coordinate of xx.
                xx = (double) x / scale;
                // coordinate (0.0,0.0) is in the center of pixel (0,0),
                // so the pixel with xc=0 get the values of xx from -0.5 to 0.5 
                xc = (int) Math.Floor( xx + 0.5 );
                gaussian_kernel( kernel, sigma, (double) h + xx - (double) xc );
                // the kernel must be computed for each x because the fine
                // offset xx-xc is different in each case 
                for(y=0;y<aux.height;y++)
                {
                    sum = 0.0;
                    for(i=0;i<dim;i++)
                    {
                        j = (int)(xc - h + i);
                        // symmetry boundary condition
                        while( j < 0 ) j += double_x_size;
                        while( j >= double_x_size ) j -= double_x_size;
                        if( j >= (int) inImage.width ) j = double_x_size-1-j;
                        sum += inImage.data[ j , y ] * kernel[i];
                    }
                    aux.data[ x , y  ] = sum;
                }
            }
            // Second subsampling: y axis 
            for(y=0;y<outImage.height;y++)
            {
                // y   is the coordinate in the new image.
                // yy  is the corresponding x-value in the original size image.
                // yc  is the integer value, the pixel coordinate of xx.
                yy = (double) y / scale;
                // coordinate (0.0,0.0) is in the center of pixel (0,0),
                // so the pixel with yc=0 get the values of yy from -0.5 to 0.5 
                yc = (int) Math.Floor( yy + 0.5 );
                gaussian_kernel( kernel, sigma, (double) h + yy - (double) yc );
                // the kernel must be computed for each y because the fine
                // offset yy-yc is different in each case 
                for(x=0;x<outImage.width;x++)
                {
                    sum = 0.0;
                    for(i=0;i<dim;i++)
                    {
                        j =(int)( yc - h + i);
                        // symmetry boundary condition 
                        while( j < 0                    ) j += double_y_size;
                        while( j >= double_y_size       ) j -= double_y_size;
                        if   ( j >= (int) inImage.height ) j  = double_y_size-1-j;
                        sum += aux.data[ x , j  ] * kernel[i];
                    }
                    outImage.data[ x , y ] = sum;
                }
            }
            return outImage;
        }  // End of gaussian_sampler

        /*----------------------------------------------------------------------------*/
        /*--------------------------------- Gradient ---------------------------------*/
        /*----------------------------------------------------------------------------*/
        /** Computes the direction of the level line of 'in' at each point.

            The result is:
            - an image_double with the angle at each pixel, or NOTDEF if not defined.
            - the image_double 'modgrad' (a pointer is passed as argument)
              with the gradient magnitude at each point.
            - a list of pixels 'list_p' roughly ordered by decreasing
              gradient magnitude. (The order is made by classifying points
              into bins by gradient magnitude. The parameters 'n_bins' and
              'max_grad' specify the number of bins and the gradient modulus
              at the highest bin. The pixels in the list would be in
              decreasing gradient magnitude, up to a precision of the size of
              the bins.)
            - a pointer 'mem_p' to the memory used by 'list_p' to be able to
              free the memory when it is not used anymore.
         */
        static public image<double> ll_angle(image<double> inImg  ,double threshold, uint n_bins,
                                         out image<double> modgrad, out List<coord> list_p )
        {
            image<double> g;
            uint n,p,x,y,i;
            double com1,com2,gx,gy,norm,norm2;
            double max_grad = 0.0;
            // check parameters */
            if( inImg == null || inImg.data == null || inImg.width == 0 || inImg.height == 0 ) error("ll_angle: invalid image.");
            if ( threshold < 0.0 ) error("ll_angle: 'threshold' must be positive.");
            if( n_bins == 0      ) error("ll_angle: 'n_bins' must be positive.");
            // image size shortcuts 
            n = inImg.height;
            p = inImg.width;
         
            g       = image<double>.newImage(p,n);        // allocate output image 
            modgrad = image<double>.newImage(p,n);  // get memory for the image of gradient modulus 

            // 'undefined' on the down and right boundaries 
            for(x=0;x<p;x++) g.data[x  ,n-1] = NOTDEF;
            for(y=0;y<n;y++) g.data[p-1,y  ] = NOTDEF;

            // compute gradient on the remaining pixels 
            for(x=0;x<p-1;x++)
                for(y=0;y<n-1;y++)
                {
                    //    Norm 2 computation using 2x2 pixel window:
                    //        A B
                    //        C D
                    //    and
                    //        com1 = D-A,  com2 = B-C.
                    //    Then
                    //        gx = B+D - (A+C)   horizontal difference
                    //        gy = C+D - (A+B)   vertical difference
                    //    com1 and com2 are just to avoid 2 additions.

                    com1 = inImg.data[x+1,y+1] - inImg.data[x,y  ];
                    com2 = inImg.data[x+1,y  ] - inImg.data[x,y+1];
                    gx = com1+com2;                     // gradient x component 
                    gy = com1-com2;                     // gradient y component 
                    norm2 = gx*gx+gy*gy;
                    norm = Math.Sqrt( norm2 / 4.0 );    // gradient norm 
                    modgrad.data[x,y] = norm;           // store gradient norm 
                    if( norm <= threshold )             // norm too small, gradient no defined 
                        g.data[x,y] = NOTDEF;           // gradient angle not defined 
                    else
                    { 
                        g.data[x,y] = Math.Atan2(gx,-gy);       // gradient angle computation 
                        if( norm > max_grad ) max_grad = norm;  // look for the maximum of the gradient 
                    }
                }
            // the rest of the variables are used for pseudo-ordering the gradient magnitude values 
            List<coord>[] range_l;  // array of pointers to start of bin list
            range_l = new List<coord>[n_bins];
            if (range_l == null) error("not enough memory.");
            for (i = 0; i < n_bins; i++) range_l[i] = new List<coord>();
            // compute histogram of gradient values
            double factor = n_bins / max_grad;
            for(x=0;x<p-1;x++)
                for(y=0;y<n-1;y++)
                {
                    norm = modgrad.data[x,y];
                    i = (uint) (norm * factor);  // store the point in the right bin according to its norm 
                    if( i >= n_bins ) i = n_bins-1;
                    coord co = new coord();
                    co.x = (int)x;
                    co.y = (int)y;
                    range_l[i].Add(co);
                }

             // Make the list of pixels (almost) ordered by norm value.
             // It starts by the larger bin, so the list starts by the
             // pixels with the highest gradient value. Pixels would be ordered
             // by norm value, up to a precision given by max_grad/n_bins.
             list_p = range_l[n_bins-1];
             for (i = n_bins - 1; i > 0; i--) list_p.AddRange(range_l[i-1]);
             return g;
        }

        //----------------------------------------------------------------------------
        // Is point (x,y) aligned to angle theta, up to precision 'prec'?
        static bool isaligned(int x, int y, image<double> angles, double theta,double prec)
        {
            double a;
            if (angles == null || angles.data == null)                               error("isaligned: invalid image 'angles'.");
            if (x < 0 || y < 0 || x >= (int)angles.width || y >= (int)angles.height)  error("isaligned: (x,y) out of the image.");
            if (prec < 0.0)                                                          error("isaligned: 'prec' must be positive.");
            a = angles.data[x , y];   // angle at pixel (x,y) 
            // pixels whose level-line angle is not defined are considered as NON-aligned 
            // there is no need to call the function 'double_equal' here because there is
            // no risk of problems related to the comparison doubles, we are only interested in the exact NOTDEF value 
            if (a == NOTDEF) return false; 
            // it is assumed that 'theta' and 'a' are in the range [-pi,pi] 
            theta -= a;
            if (theta < 0.0) theta = -theta;
            if (theta > M_3_2_PI)
            {
                theta -= M_2__PI;
                if (theta < 0.0) theta = -theta;
            }
            return theta <= prec;
        }

        //----------------------------------------------------------------------------
        // Absolute value angle difference.
        static double angle_diff(double a, double b)
        {
            a -= b;
            while (a <= -M_PI) a += M_2__PI;
            while (a > M_PI) a -= M_2__PI;
            if (a < 0.0) a = -a;
            return a;
        }

        //----------------------------------------------------------------------------
        // Signed angle difference.
        static double angle_diff_signed(double a, double b)
        {
            a -= b;
            while (a <= -M_PI) a += M_2__PI;
            while (a > M_PI) a -= M_2__PI;
            return a;
        }


        #region ------ NFA ------
        //----------------------------- NFA computation ------------------------------
        // Computes the natural logarithm of the absolute value of the gamma function of x using the Lanczos approximation.
        // See http://www.rskey.org/gamma.htm
        // The formula used is
        // \Gamma(x) = \frac{ \sum_{n=0}^{N} q_n x^n }{ \Pi_{n=0}^{N} (x+n) }
        //          (x+5.5)^{x+0.5} e^{-(x+5.5)}
        //    so
        //  \log\Gamma(x) = \log\left( \sum_{n=0}^{N} q_n x^n \right)
        //              + (x+0.5) \log(x+5.5) - (x+5.5) - \sum_{n=0}^{N} \log(x+n)
        //    and
        //  q0 = 75122.6331530,
        //  q1 = 80916.6278952,
        //  q2 = 36308.2951477,
        //  q3 = 8687.24529705,
        //  q4 = 1168.92649479,
        //  q5 = 83.8676043424,
        //  q6 = 2.50662827511.
        static double[] NFA_q = new double[7] { 75122.6331530, 80916.6278952, 36308.2951477,
                                                8687.24529705, 1168.92649479, 83.8676043424,2.50662827511 };
        static double log_gamma_lanczos(double x)
        {
            double a = (x+0.5) * Math.Log(x+5.5) - (x+5.5);
            double b = 0.0;
            int n;
            for(n=0;n<7;n++)
            {
                a -= Math.Log( x + (double) n );
                b += NFA_q[n] * Math.Pow( x, (double) n );
            }
            return a + Math.Log(b);
        }

        //----------------------------------------------------------------------------
        // Computes the natural logarithm of the absolute value of
        //    the gamma function of x using Windschitl method.
        //    See http://www.rskey.org/gamma.htm
        //    The formula used is
        //        \Gamma(x) = \sqrt{\frac{2\pi}{x}} \left( \frac{x}{e}
        //                    \sqrt{ x\sinh(1/x) + \frac{1}{810x^6} } \right)^x
        //    so
        //        \log\Gamma(x) = 0.5\log(2\pi) + (x-0.5)\log(x) - x
        //                      + 0.5x\log\left( x\sinh(1/x) + \frac{1}{810x^6} \right).
        //    This formula is a good approximation when x > 15.
        static double log_gamma_windschitl(double x)
        {
              return 0.918938533204673 + (x-0.5)*Math.Log(x) - x
                     + 0.5*x*Math.Log( x*Math.Sinh(1/x) + 1/(810.0*Math.Pow(x,6.0)) );
        }

        //----------------------------------------------------------------------------
        // Computes the natural logarithm of the absolute value of
        //    the gamma function of x. When x>15 use log_gamma_windschitl(),
        //    otherwise use log_gamma_lanczos().
        static double log_gamma(double x)
        {
            return (x)>15.0?log_gamma_windschitl(x):log_gamma_lanczos(x);
        }

        //----------------------------------------------------------------------------*/
        // Computes -log10(NFA).
        //
        // NFA stands for Number of False Alarms:
        // \mathrm{NFA} = NT \cdot B(n,k,p)
        //
        // - NT       - number of tests
        // - B(n,k,p) - tail of binomial distribution with parameters n,k and p:
        //   B(n,k,p) = \sum_{j=k}^n
        //              \left(\begin{array}{c}n\\j\end{array}\right)
        //              p^{j} (1-p)^{n-j}
        //
        // The value -log10(NFA) is equivalent but more intuitive than NFA:
        // - -1 corresponds to 10 mean false alarms
        // -  0 corresponds to 1 mean false alarm
        // -  1 corresponds to 0.1 mean false alarms
        // -  2 corresponds to 0.01 mean false alarms
        // -  ...
        //
        // Used this way, the bigger the value, better the detection,
        // and a logarithmic scale is used.
        //
        // @param n,k,p binomial parameters.
        // @param logNT logarithm of Number of Tests
        //
        // The computation is based in the gamma function by the following
        // relation:
        //        \left(\begin{array}{c}n\\k\end{array}\right)
        //      = \frac{ \Gamma(n+1) }{ \Gamma(k+1) \cdot \Gamma(n-k+1) }.
        // We use efficient algorithms to compute the logarithm of
        // the gamma function.
        //
        // To make the computation faster, not all the sum is computed, part
        // of the terms are neglected based on a bound to the error obtained
        // (an error of 10% in the result is accepted).

        // Size of the table to store already computed inverse values.
        const int TABSIZE = 100000;
        static double[] inv=new double[TABSIZE];   // table to keep computed inverse values 
        static double nfa(int n, int k, double p, double logNT)
        {
              double tolerance = 0.1;       /* an error of 10% in the result is accepted */
              double log1term,term,bin_term,mult_term,bin_tail,err,p_term;
              int i;

              if( n<0 || k<0 || k>n || p<=0.0 || p>=1.0 ) error("nfa: wrong n, k or p values.");
              if( n==0 || k==0 ) return -logNT;
              if( n==k ) return -logNT - (double) n * Math.Log10(p);

              p_term = p / (1.0 - p);   // probability term

              // compute the first term of the series 
              //
              //   binomial_tail(n,k,p) = sum_{i=k}^n bincoef(n,i) * p^i * (1-p)^{n-i}
              //   where bincoef(n,i) are the binomial coefficients.
              //   But
              //     bincoef(n,k) = gamma(n+1) / ( gamma(k+1) * gamma(n-k+1) ).
              //   We use this to compute the first term. Actually the log of it.
              log1term = log_gamma( (double) n + 1.0 ) - log_gamma( (double) k + 1.0 )
                       - log_gamma( (double) (n-k) + 1.0 )
                       + (double) k * Math.Log(p) + (double) (n-k) * Math.Log(1.0-p);
              term = Math.Exp(log1term);

              // in some cases no more computations are needed 
              if( double_equal(term,0.0) )              // the first term is almost zero 
                {
                  if( (double) k > (double) n * p )     // at begin or end of the tail?  
                    return -log1term / M_LN10 - logNT;  // end: use just the first term  
                  else
                    return -logNT;                      // begin: the tail is roughly 1  
                }

              // compute more terms if needed 
              bin_tail = term;
              for(i=k+1;i<=n;i++)
              {
                //As
                //  term_i = bincoef(n,i) * p^i * (1-p)^(n-i)
                //and
                //  bincoef(n,i)/bincoef(n,i-1) = n-1+1 / i,
                //then,
                //  term_i / term_i-1 = (n-i+1)/i * p/(1-p)
                //and
                //  term_i = term_i-1 * (n-i+1)/i * p/(1-p).
                //1/i is stored in a table as they are computed,
                //because divisions are expensive.
                //p/(1-p) is computed only once and stored in 'p_term'.
                  bin_term = (double) (n-i+1) * ( i<TABSIZE ?
                               ( inv[i]!=0.0 ? inv[i] : ( inv[i] = 1.0 / (double) i ) ) : 1.0 / (double) i );

                  mult_term = bin_term * p_term;
                  term *= mult_term;
                  bin_tail += term;
                  if(bin_term<1.0)
                  {
                        //When bin_term<1 then mult_term_j<mult_term_i for j>i.
                        //Then, the error on the binomial tail when truncated at
                        //the i term can be bounded by a geometric series of form
                        //term_i * sum mult_term_i^j.                            */
                      err = term * ( ( 1.0 - Math.Pow( mult_term, (double) (n-i+1) ) ) /
                                     (1.0-mult_term) - 1.0 );

                      // One wants an error at most of tolerance*final_result, or:
                      //   tolerance * abs(-log10(bin_tail)-logNT).
                      //   Now, the error that can be accepted on bin_tail is
                      //   given by tolerance*final_result divided by the derivative
                      //   of -log10(x) when x=bin_tail. that is:
                      //   tolerance * abs(-log10(bin_tail)-logNT) / (1/bin_tail)
                      //   Finally, we truncate the tail if the error is less than:
                      //   tolerance * abs(-log10(bin_tail)-logNT) * bin_tail        
                      if( err < tolerance * Math.Abs(-Math.Log10(bin_tail)-logNT) * bin_tail ) break;
                   }
                }
                return -Math.Log10(bin_tail) - logNT;
        }
        #endregion

        #region Rectange Region 
        //--------------------------- Rectangle structure ----------------------------
        // Rectangle structure: line segment with width.
        class rect
        {
            public double x1,y1,x2,y2;  // first and second point of the line segment 
            public double width;        // rectangle width 
            public double x,y;          // center of the rectangle 
            public double theta;        // angle 
            public double dx,dy;        // (dx,dy) is vector oriented as the line segment 
            public double prec;         // tolerance angle 
            public double p;            // probability of a point with angle within 'prec'
            public void copyFrom(rect source)
            {
                x1=source.x1;
                y1=source.y1;
                x2=source.x2;
                y2=source.y2;
                width=source.width;
                x=source.x;
                y=source.y;
                theta=source.theta;
                dx=source.dx;
                dy=source.dy;
                prec=source.prec;
                p=source.p;
            }
            public void move(double x,double y) { x1+=x; x2+=x; y1+=y; y2+=y; }
            public void zoom(double scale)
            {
                if (scale == 1.0) return;
                x1 /= scale; y1 /= scale;
                x2 /= scale; y2 /= scale; width /= scale;
            }
        };

        //----------------------------------------------------------------------------
        //  Rectangle points iterator.
        //
        //The integer coordinates of pixels inside a rectangle are
        //iteratively explored. This structure keep track of the process and
        //functions ri_ini(), ri_inc(), ri_end(), and ri_del() are used in
        //the process. An example of how to use the iterator is as follows:
        //
        //  struct rect * rec = XXX; // some rectangle
        //  rect_iter * i;
        //  for( i=ri_ini(rec); !ri_end(i); ri_inc(i) )
        //    {
        //      // your code, using 'i->x' and 'i->y' as coordinates
        //    }
        //  ri_del(i); // delete iterator
        //
        //The pixels are explored 'column' by 'column', where we call
        //'column' a set of pixels with the same x value that are inside the
        //rectangle. The following is an schematic representation of a
        //rectangle, the 'column' being explored is marked by colons, and
        //the current pixel being explored is 'x,y'.
        //          vx[1],vy[1]
        //             *   *
        //            *       *
        //           *           *
        //          *               ye
        //         *                :  *
        //    vx[0],vy[0]           :     *
        //           *              :        *
        //              *          x,y          *
        //                 *        :              *
        //                    *     :            vx[2],vy[2]
        //                       *  :                *
        //    y                     ys              *
        //    ^                        *           *
        //    |                           *       *
        //    |                              *   *
        //    +---> x                      vx[3],vy[3]
        //The first 'column' to be explored is the one with the smaller x
        //value. Each 'column' is explored starting from the pixel of the
        //'column' (inside the rectangle) with the smallest y value.

        //The four corners of the rectangle are stored in order that rotates
        //around the corners at the arrays 'vx[]' and 'vy[]'. The first
        //point is always the one with smaller x value.

        //'x' and 'y' are the coordinates of the pixel being explored. 'ys'
        //and 'ye' are the start and end values of the current column being
        //explored. So, 'ys' < 'ye'.
        class rect_iter
        {
            public double vx0,vx1,vx2,vx3;  // rectangle's corner X coordinates in circular order 
            public double vy0,vy1,vy2,vy3;  // rectangle's corner Y coordinates in circular order 
            public double ys,ye;  // start and end Y values of current 'column' 
            public int x,y;       // coordinates of currently explored pixel 

            // Check if the iterator finished the full iteration.
            public bool end()
            {   // if the current x value is larger than the largest
                // x value in the rectangle (vx[2]), we know the full
                // exploration of the rectangle is finished. 
                return (double)(x) > vx2;
            }
            
            // Increment a rectangle iterator.
            public void inc()
            {   // if not at end of exploration, increase y value for next pixel in the 'column' 
                if (!end()) y++;
                // if the end of the current 'column' is reached, and it is not the end of exploration,
                // advance to the next 'column'
                while( (double) (y) > ye && !end() )
                {   
                    x++;                 // increase x, next 'column' 
                    if( end() ) return;  // if end of exploration, return 

                    // update lower y limit (start) for the new 'column'.
                    // We need to interpolate the y value that corresponds to the
                    // lower side of the rectangle. The first thing is to decide if
                    // the corresponding side is
                    //  vx[0],vy[0] to vx[3],vy[3] or
                    //  vx[3],vy[3] to vx[2],vy[2]
                    // Then, the side is interpolated for the x value of the
                    // 'column'. But, if the side is vertical (as it could happen if
                    // the rectangle is vertical and we are dealing with the first
                    // or last 'columns') then we pick the lower value of the side
                    // by using 'inter_low'.
                    if( (double) x < vx3 )
                        ys = inter_low((double)x,vx0,vy0,vx3,vy3);
                    else
                        ys = inter_low((double)x,vx3,vy3,vx2,vy2);

                    // update upper y limit (end) for the new 'column'.
                    //We need to interpolate the y value that corresponds to the
                    //upper side of the rectangle. The first thing is to decide if
                    //the corresponding side is

                    //  vx[0],vy[0] to vx[1],vy[1] or
                    //  vx[1],vy[1] to vx[2],vy[2]

                    //Then, the side is interpolated for the x value of the
                    //'column'. But, if the side is vertical (as it could happen if
                    //the rectangle is vertical and we are dealing with the first
                    //or last 'columns') then we pick the lower value of the side
                    //by using 'inter_low'.
                    if( (double)x < vx1 )
                        ye = inter_hi((double)x,vx0,vy0,vx1,vy1);
                    else
                        ye = inter_hi((double)x,vx1,vy1,vx2,vy2);
                    y = (int) Math.Ceiling(ys);  // new y 
            }
        }

            // Create and initialize a rectangle iterator.
            public static rect_iter initFrom(rect r)
            {
                double[] vx=new double[4];
                double[] vy=new double[4];
                int offset;
                if( r == null ) error("ri_ini: invalid rectangle.");
                // build list of rectangle corners ordered in a circular way around the rectangle
                vx[0] = r.x1 - r.dy * r.width / 2.0;
                vy[0] = r.y1 + r.dx * r.width / 2.0;
                vx[1] = r.x2 - r.dy * r.width / 2.0;
                vy[1] = r.y2 + r.dx * r.width / 2.0;
                vx[2] = r.x2 + r.dy * r.width / 2.0;
                vy[2] = r.y2 - r.dx * r.width / 2.0;
                vx[3] = r.x1 + r.dy * r.width / 2.0;
                vy[3] = r.y1 - r.dx * r.width / 2.0;

                // compute rotation of index of corners needed so that the first point has the smaller x.
                // if one side is vertical, thus two corners have the same smaller x
                // value, the one with the largest y value is selected as the first.
                if     ( r.x1 <  r.x2 && r.y1 <= r.y2 ) offset = 0;
                else if( r.x1 >= r.x2 && r.y1 <  r.y2 ) offset = 1;
                else if( r.x1 >  r.x2 && r.y1 >= r.y2 ) offset = 2;
                else                                    offset = 3;
                 // apply rotation of index.
                 rect_iter it=new rect_iter();
                 it.vx0 = vx[(offset  )%4];
                 it.vy0 = vy[(offset  )%4];
                 it.vx1 = vx[(offset+1)%4];
                 it.vy1 = vy[(offset+1)%4];
                 it.vx2 = vx[(offset+2)%4];
                 it.vy2 = vy[(offset+2)%4];
                 it.vx3 = vx[(offset+3)%4];
                 it.vy3 = vy[(offset+3)%4];

                // Set an initial condition.
                // The values are set to values that will cause 'ri_inc' (that will
                // be called immediately) to initialize correctly the first 'column'
                // and compute the limits 'ys' and 'ye'.
                // 'y' is set to the integer value of vy[0], the starting corner.
                // 'ys' and 'ye' are set to very small values, so 'ri_inc' will
                // notice that it needs to start a new 'column'.
                // The smallest integer coordinate inside of the rectangle is
                // 'ceil(vx[0])'. The current 'x' value is set to that value minus
                // one, so 'ri_inc' (that will increase x by one) will advance to
                // the first 'column'.
                it.x = (int) Math.Ceiling(it.vx0) - 1;
                it.y = (int) Math.Ceiling(it.vy0);
                it.ys = it.ye = Double.MinValue;
                it.inc();  // advance to the first pixel 
                return it;
            }

            //----------------------------------------------------------------------------
            // Interpolate y value corresponding to 'x' value given, in
            // the line 'x1,y1' to 'x2,y2'; if 'x1=x2' return the smaller
            // of 'y1' and 'y2'.
            // The following restrictions are required:
            // - x1 <= x2
            // - x1 <= x
            // - x  <= x2
            static double inter_low(double x, double x1, double y1, double x2, double y2)
            {
                if( x1 > x2 || x < x1 || x > x2 )
                error("inter_low: unsuitable input, 'x1>x2' or 'x<x1' or 'x>x2'.");
                // interpolation 
                if( double_equal(x1,x2) && y1<y2 ) return y1;
                if( double_equal(x1,x2) && y1>y2 ) return y2;
                return y1 + (x-x1) * (y2-y1) / (x2-x1);
            }

            //----------------------------------------------------------------------------
            //  Interpolate y value corresponding to 'x' value given, in
            //    the line 'x1,y1' to 'x2,y2'; if 'x1=x2' return the larger
            //    of 'y1' and 'y2'.
            //    The following restrictions are required:
            //    - x1 <= x2
            //    - x1 <= x
            //    - x  <= x2
            static double inter_hi(double x, double x1, double y1, double x2, double y2)
            {
                if (x1 > x2 || x < x1 || x > x2 ) error("inter_hi: unsuitable input, 'x1>x2' or 'x<x1' or 'x>x2'.");
                // interpolation 
                if (double_equal(x1,x2) && y1<y2 ) return y2;
                if (double_equal(x1,x2) && y1>y2 ) return y1;
                return y1 + (x-x1) * (y2-y1) / (x2-x1);
            }

        } ;

        // Compute a rectangle's NFA value.
        static double rect_nfa(rect rec, image<double> angles, double logNT)
        {
            rect_iter i;
            int pts = 0, alg = 0;
            if( rec    == null ) error("rect_nfa: invalid rectangle.");
            if( angles == null ) error("rect_nfa: invalid 'angles'.");
            // compute the total number of pixels and of aligned points in 'rec' 
            for(i=rect_iter.initFrom(rec); !i.end(); i.inc()) // rectangle iterator 
            if ( i.x >= 0 && i.y >= 0 && i.x < (int) angles.width && i.y < (int) angles.height )
            {
                ++pts; // total number of pixels counter 
                if( isaligned(i.x, i.y, angles, rec.theta, rec.prec) ) ++alg; // aligned points counter 
            }
            return nfa(pts,alg,rec.p,logNT); // compute NFA value 
        }


        //---------------------------------- Regions ---------------------------------
        // Compute region's angle as the principal inertia axis of the region.
        // The following is the region inertia matrix A:
        //     A = \left(\begin{array}{cc}
        //                                 Ixx & Ixy \\
        //                                 Ixy & Iyy \\
        //          \end{array}\right)
        // where
        //   Ixx =   sum_i G(i).(y_i - cx)^2
        //   Iyy =   sum_i G(i).(x_i - cy)^2
        //   Ixy = - sum_i G(i).(x_i - cx).(y_i - cy)
        // and
        // - G(i) is the gradient norm at pixel i, used as pixel's weight.
        // - x_i and y_i are the coordinates of pixel i.
        // - cx and cy are the coordinates of the center of th region.
        // lambda1 and lambda2 are the eigenvalues of matrix A,
        // with lambda1 >= lambda2. They are found by solving the
        // characteristic polynomial:
        //   det( lambda I - A) = 0
        // that gives:
        //   lambda1 = ( Ixx + Iyy + sqrt( (Ixx-Iyy)^2 + 4.0*Ixy*Ixy) ) / 2
        //   lambda2 = ( Ixx + Iyy - sqrt( (Ixx-Iyy)^2 + 4.0*Ixy*Ixy) ) / 2
        // To get the line segment direction we want to get the angle the
        // eigenvector associated to the smallest eigenvalue. We have
        // to solve for a,b in:
        //   a.Ixx + b.Ixy = a.lambda2
        //   a.Ixy + b.Iyy = b.lambda2
        // We want the angle theta = atan(b/a). It can be computed with
        // any of the two equations:
        //  theta = atan( (lambda2-Ixx) / Ixy )
        // or
        //  theta = atan( Ixy / (lambda2-Iyy) )
        // When |Ixx| > |Iyy| we use the first, otherwise the second (just to
        // get better numeric precision).

        struct point
        {
            public int x;
            public int y;
        }

        class regionLSD
        {
            image<double> modgrad;
            image<double> angles;
            image<bool>   used;
            image<int>    region=null;     // 
            public image<int>    RegionImage { get { return region; } }
            uint width, height;
            public bool setupImage(image<double> grade, image<double> angs,bool markRegionToImageInt)
            {
                if (grade.height != angs.height || grade.width != angs.width) return false;
                width = grade.width;   height = grade.height;
                modgrad = grade;        angles = angs;
                used = image<bool>.newImage(width, height);
                reg = new point[width * height];
                rec = new rect();
                if (markRegionToImageInt)
                    region = image<int>.newImage(width, height);
                return true;
            }
            public bool UsedOrNodef(int x, int y)
            {
                if (used.data[x, y]) return true;
                if (angles.data[x, y] != NOTDEF) return false;
                return true;
            }
            
            point[] reg;
            double  reg_angle;
            int     reg_size;
            public int Size { get { return reg_size; } }
            public rect    rec;

            public double getTheta(double x, double y, double prec)
            {
                double lambda, theta, weight;
                double Ixx = 0.0, Iyy = 0.0, Ixy = 0.0;
                if (reg_size <= 1)                           error("get_theta: region size <= 1.");
                if (modgrad == null || modgrad.data == null) error("get_theta: invalid 'modgrad'.");
                if (prec < 0.0)                              error("get_theta: 'prec' must be positive.");
                for (int i = 0; i < reg_size; i++)   // compute inertia matrix 
                {
                    double x1 = reg[i].x;
                    double y1 = reg[i].y;
                    weight = modgrad.data[reg[i].x, reg[i].y];
                    Ixx += (y1 - y) * (y1 - y) * weight;
                    Iyy += (x1 - x) * (x1 - x) * weight;
                    Ixy -= (x1 - x) * (y1 - y) * weight;
                }
                if (double_equal(Ixx, 0.0) && double_equal(Iyy, 0.0) && double_equal(Ixy, 0.0)) error("get_theta: null inertia matrix.");
                lambda = 0.5 * (Ixx + Iyy - Math.Sqrt((Ixx - Iyy) * (Ixx - Iyy) + 4.0 * Ixy * Ixy));                  // compute smallest eigenvalue 
                theta = Math.Abs(Ixx) > Math.Abs(Iyy) ? Math.Atan2(lambda - Ixx, Ixy) : Math.Atan2(Ixy, lambda - Iyy);  // compute angle 
                // The previous procedure doesn't cares about orientation,
                // so it could be wrong by 180 degrees. Here is corrected if necessary.
                if (angle_diff(theta, reg_angle) > prec) theta += M_PI;
                return theta;
            }
            public void   grow    (int x   , int y   , double prec)
            {
                if (angles == null || angles.data == null) error("region_grow: invalid image 'angles'.");
                if (x < 0 || y < 0 || x >= (int)angles.width || y >= (int)angles.height) error("region_grow: (x,y) out of the image.");
                if (reg == null) error("region_grow: invalid 'reg'.");
                if (used == null || used.data == null) error("region_grow: invalid image 'used'.");
                // first point of the region 
                reg_size = 1;
                reg[0].x = x;
                reg[0].y = y;
                reg_angle = angles.data[x, y];  // region's angle 

                double sumdx, sumdy;
                int xx, yy, i;
                sumdx = Math.Cos(reg_angle);
                sumdy = Math.Sin(reg_angle);
                used.data[x, y] = true;
                // try neighbors as new region points 
                for (i = 0; i < reg_size; i++)
                    for (xx = reg[i].x - 1; xx <= reg[i].x + 1; xx++)
                        for (yy = reg[i].y - 1; yy <= reg[i].y + 1; yy++)
                            if (xx >= 0 && yy >= 0 && xx < (int)used.width && yy < (int)used.height
                                && used.data[xx, yy] != true && isaligned(xx, yy, angles, reg_angle, prec))
                            {   // add point 
                                used.data[xx, yy] = true;
                                reg[reg_size].x = xx;
                                reg[reg_size].y = yy;
                                ++(reg_size);
                                // update region's angle 
                                sumdx += Math.Cos(angles.data[xx, yy]);
                                sumdy += Math.Sin(angles.data[xx, yy]);
                                reg_angle = Math.Atan2(sumdy, sumdx);
                            }
            }
            public void toRect      (double prec, double p)
            {
                double x, y, dx, dy, l, w, theta, weight, sum, l_min, l_max, w_min, w_max;
                int i;
                if (reg == null)    error("region2rect: invalid region.");
                if (reg_size <= 1)  error("region2rect: region size <= 1.");
                if (rec == null)    error("region2rect: invalid 'rec'.");
                if (modgrad == null || modgrad.data == null) error("region2rect: invalid image 'modgrad'.");

                // center of the region:
                // It is computed as the weighted sum of the coordinates
                // of all the pixels in the region. The norm of the gradient
                // is used as the weight of a pixel. The sum is as follows:
                // cx = \sum_i G(i).x_i
                // cy = \sum_i G(i).y_i
                // where G(i) is the norm of the gradient of pixel i
                // and x_i,y_i are its coordinates.
                x = y = sum = 0.0;
                for (i = 0; i < reg_size; i++)
                {
                    weight = modgrad.data[reg[i].x, reg[i].y];
                    x += (double)reg[i].x * weight;
                    y += (double)reg[i].y * weight;
                    sum += weight;
                }
                if (sum <= 0.0) error("region2rect: weights sum equal to zero.");
                x /= sum;
                y /= sum;
                theta = getTheta( x, y, prec);     // theta 
                // length and width:
                // 'l' and 'w' are computed as the distance from the center of the
                // region to pixel i, projected along the rectangle axis (dx,dy) and
                // to the orthogonal axis (-dy,dx), respectively.
                // The length of the rectangle goes from l_min to l_max, where l_min
                // and l_max are the minimum and maximum values of l in the region.
                // Analogously, the width is selected from w_min to w_max, where
                // w_min and w_max are the minimum and maximum of w for the pixels
                // in the region.
                dx = Math.Cos(theta);
                dy = Math.Sin(theta);
                l_min = l_max = w_min = w_max = 0.0;
                for (i = 0; i < reg_size; i++)
                {
                    l = ((double)reg[i].x - x) * dx + ((double)reg[i].y - y) * dy;
                    w = -((double)reg[i].x - x) * dy + ((double)reg[i].y - y) * dx;
                    if (l > l_max) l_max = l;
                    if (l < l_min) l_min = l;
                    if (w > w_max) w_max = w;
                    if (w < w_min) w_min = w;
                }
                // store values 
                rec.x1 = x + l_min * dx;
                rec.y1 = y + l_min * dy;
                rec.x2 = x + l_max * dx;
                rec.y2 = y + l_max * dy;
                rec.width = w_max - w_min;
                rec.x = x;
                rec.y = y;
                rec.theta = theta;
                rec.dx = dx;
                rec.dy = dy;
                rec.prec = prec;
                rec.p = p;
                //    we impose a minimal width of one pixel
                //    A sharp horizontal or vertical step would produce a perfectly
                //    horizontal or vertical region. The width computed would be
                //    zero. But that corresponds to a one pixels width transition in
                //    the image.
                if (rec.width < 1.0) rec.width = 1.0;
            }
            public bool reduceRadius(double prec, double p, double density_th)
            {
                double density, rad1, rad2, rad, xc, yc;
                int i;
                if (reg == null) error("reduce_region_radius: invalid pointer 'reg'.");
                //if( reg_size == null )  error("reduce_region_radius: invalid pointer 'reg_size'.");
                if (prec < 0.0) error("reduce_region_radius: 'prec' must be positive.");
                if (rec == null) error("reduce_region_radius: invalid pointer 'rec'.");
                if (used == null || used.data == null) error("reduce_region_radius: invalid image 'used'.");
                if (angles == null || angles.data == null) error("reduce_region_radius: invalid image 'angles'.");

                density = (double)reg_size / (dist(rec.x1, rec.y1, rec.x2, rec.y2) * rec.width);  // compute region points density 
                if (density >= density_th) return true;  // if the density criterion is satisfied there is nothing to do 
                xc = (double)reg[0].x;                   // compute region's radius 
                yc = (double)reg[0].y;
                rad1 = dist(xc, yc, rec.x1, rec.y1);
                rad2 = dist(xc, yc, rec.x2, rec.y2);
                rad = rad1 > rad2 ? rad1 : rad2;
                while (density < density_th)             // while the density criterion is not satisfied, remove farther pixels 
                {
                    rad *= 0.75;                         // reduce region's radius to 75% of its value 
                    for (i = 0; i < reg_size; i++)       // remove points from the region and update 'used' map 
                        if (dist(xc, yc, (double)reg[i].x, (double)reg[i].y) > rad)
                        {
                            used.data[reg[i].x, reg[i].y] = false;           // point not kept, mark it as NOTUSED 
                            // remove point from the region 
                            reg[i].x = reg[reg_size - 1].x;                  // if i==reg_size-1 copy itself  ????
                            reg[i].y = reg[reg_size - 1].y;
                            --(reg_size);
                            --i;                                             // to avoid skipping one point 
                        }
                    // reject if the region is too small.2 is the minimal region size for 'region2rect' to work. 
                    if (reg_size < 2) return false;
                    toRect(prec, p);                                                                   // re-compute rectangle
                    density = (double)reg_size / (dist(rec.x1, rec.y1, rec.x2, rec.y2) * rec.width);   // re-compute region points density 
                }
                return true;   // if this point is reached, the density criterion is satisfied 
            }
            public bool refine      (double prec, double p, double density_th)
            {
                double angle, ang_d, mean_angle, tau, density, xc, yc, ang_c, sum, s_sum;
                int i, n;
                if (reg == null) error("refine: invalid pointer 'reg'.");
                //if( reg_size == null )    error("refine: invalid pointer 'reg_size'.");
                if (prec < 0.0) error("refine: 'prec' must be positive.");
                if (rec == null) error("refine: invalid pointer 'rec'.");
                if (used == null || used.data == null) error("refine: invalid image 'used'.");
                if (angles == null || angles.data == null) error("refine: invalid image 'angles'.");
                density = (double)reg_size / (dist(rec.x1, rec.y1, rec.x2, rec.y2) * rec.width);  // compute region points density
                if (density >= density_th) return true;                                           // if the density criterion is satisfied there is nothing to do 

                //------ First try: reduce angle tolerance ------
                // compute the new mean angle and tolerance 
                xc = (double)reg[0].x;
                yc = (double)reg[0].y;
                ang_c = angles.data[reg[0].x, reg[0].y];
                sum = s_sum = 0.0;
                n = 0;
                for (i = 0; i < reg_size; i++)
                {
                    used.data[reg[i].x, reg[i].y] = false;
                    if (dist(xc, yc, (double)reg[i].x, (double)reg[i].y) < rec.width)
                    {
                        angle = angles.data[reg[i].x, reg[i].y];
                        ang_d = angle_diff_signed(angle, ang_c);
                        sum += ang_d;
                        s_sum += ang_d * ang_d;
                        ++n;
                    }
                }
                mean_angle = sum / (double)n;
                tau = 2.0 * Math.Sqrt((s_sum - 2.0 * mean_angle * sum) / (double)n + mean_angle * mean_angle);  // 2 * standard deviation
                // find a new region from the same starting point and new angle tolerance 
                grow(reg[0].x, reg[0].y, tau);
                if (reg_size < 2) return false;                                                     // if the region is too small, reject 
                toRect( prec, p);                                                                   // re-compute rectangle 
                density = (double)reg_size / (dist(rec.x1, rec.y1, rec.x2, rec.y2) * rec.width);    // re-compute region points density 
                //------ Second try: reduce region radius ------
                if (density < density_th)
                    return reduceRadius( prec, p,density_th);
                return true;  // if this point is reached, the density criterion is satisfied 
            }
            public double rectImprove(double logNT, double log_eps)
            {
                rect r = new rect();
                double log_nfa, log_nfa_new;
                double delta = 0.5;
                double delta_2 = delta / 2.0;
                int n;
                log_nfa = rect_nfa(rec, angles, logNT);
                if (log_nfa > log_eps) return log_nfa;
                r.copyFrom(rec);   // try finer precisions 
                for (n = 0; n < 5; n++)
                {
                    r.p /= 2.0;
                    r.prec = r.p * M_PI;
                    log_nfa_new = rect_nfa(r, angles, logNT);
                    if (log_nfa_new > log_nfa)
                    {
                        log_nfa = log_nfa_new;
                        rec.copyFrom(r);
                    }
                }
                if (log_nfa > log_eps) return log_nfa;
                // try to reduce width 
                r.copyFrom(rec);
                for (n = 0; n < 5; n++)
                {
                    if ((r.width - delta) >= 0.5)
                    {
                        r.width -= delta;
                        log_nfa_new = rect_nfa(r, angles, logNT);
                        if (log_nfa_new > log_nfa)
                        {
                            rec.copyFrom(r);
                            log_nfa = log_nfa_new;
                        }
                    }
                }

                if (log_nfa > log_eps) return log_nfa;

                /* try to reduce one side of the rectangle */
                r.copyFrom(rec);
                for (n = 0; n < 5; n++)
                {
                    if ((r.width - delta) >= 0.5)
                    {
                        r.x1 += -r.dy * delta_2;
                        r.y1 += r.dx * delta_2;
                        r.x2 += -r.dy * delta_2;
                        r.y2 += r.dx * delta_2;
                        r.width -= delta;
                        log_nfa_new = rect_nfa(r, angles, logNT);
                        if (log_nfa_new > log_nfa)
                        {
                            rec.copyFrom(r);
                            log_nfa = log_nfa_new;
                        }
                    }
                }
                if (log_nfa > log_eps) return log_nfa;
                // try to reduce the other side of the rectangle
                r.copyFrom(rec);
                for (n = 0; n < 5; n++)
                {
                    if ((r.width - delta) >= 0.5)
                    {
                        r.x1 -= -r.dy * delta_2;
                        r.y1 -= r.dx * delta_2;
                        r.x2 -= -r.dy * delta_2;
                        r.y2 -= r.dx * delta_2;
                        r.width -= delta;
                        log_nfa_new = rect_nfa(r, angles, logNT);
                        if (log_nfa_new > log_nfa)
                        {
                            rec.copyFrom(r);
                            log_nfa = log_nfa_new;
                        }
                    }
                }
                if (log_nfa > log_eps) return log_nfa;
                // try even finer precisions 
                r.copyFrom(rec);
                for (n = 0; n < 5; n++)
                {
                    r.p /= 2.0;
                    r.prec = r.p * M_PI;
                    log_nfa_new = rect_nfa(r, angles, logNT);
                    if (log_nfa_new > log_nfa)
                    {
                        log_nfa = log_nfa_new;
                        rec.copyFrom(r);
                    }
                }
                return log_nfa;
            }

            public void markCurrentRegion2Image(int regionNo)
            {
                if (region != null)
                    for (int i = 0; i < reg_size; i++)
                        region.data[reg[i].x, reg[i].y] = regionNo;

            }

        }
        #endregion

        #region ------ LSD ------
        /*----------------------------------------------------------------------------*/
        /*-------------------------- Line Segment Detector ---------------------------*/
        /*----------------------------------------------------------------------------*/
        // LSD full interface.
        public static doubleTupleList LineSegmentDetection(image<double> img,
                                double scale, double sigma_scale, double quant,
                                double ang_th, double log_eps, double density_th,int n_bins,
                                ref image<int> imgReg)
        {
            if( img == null)                            error("invalid image input.");
            else if (img.width <= 0 || img.height <= 0 ) error("invalid image input size.");
            if( scale <= 0.0 )                          error("'scale' value must be positive.");
            if( sigma_scale <= 0.0 )                    error("'sigma_scale' value must be positive.");
            if( quant < 0.0 )                           error("'quant' value must be positive.");
            if( ang_th <= 0.0 || ang_th >= 180.0 )      error("'ang_th' value must be in the range (0,180).");
            if( density_th < 0.0 || density_th > 1.0 )  error("'density_th' value must be in the range [0,1].");
            if( n_bins <= 0 )                           error("'n_bins' value must be positive.");

            doubleTupleList outList = doubleTupleList.newTupleList(7);
            image<double> scaled_image,angles,modgrad;
            List<coord> list_p;
            // angle tolerance 
            double prec= M_PI * ang_th / 180.0;
            double p   = ang_th / 180.0;
            double rho = quant / Math.Sin(prec); // gradient magnitude threshold 
            // load and scale image (if necessary) and compute angle at each pixel 
            if( scale != 1.0 )
            {
                scaled_image = gaussian_sampler( img, scale, sigma_scale );
                angles = ll_angle(scaled_image, rho,(uint)n_bins,out modgrad,out list_p );
            }
            else
                angles = ll_angle( img        , rho,(uint)n_bins,out modgrad,out list_p);

            uint xsize = angles.width;
            uint ysize = angles.height;
            int min_reg_size;
            double log_nfa,logNT;
            logNT = 5.0 * ( Math.Log10( (double) xsize ) + Math.Log10( (double) ysize ) ) / 2.0 + Math.Log10(11.0);
            min_reg_size = (int) (-logNT/Math.Log10(p));    // minimal number of points in region that can give a meaningful event

            regionLSD reg = new regionLSD();
            reg.setupImage(modgrad, angles,imgReg!=null);
            int ls_count = 0;                   // line segments are numbered 1,2,3,... 
            // search for line segments 
            foreach(coord co in list_p)
                if (!reg.UsedOrNodef(co.x, co.y))                                                   // there is no risk of double comparison problems here
            {                                                                                       // because we are only interested in the exact NOTDEF value   
                reg.grow(co.x, co.y, prec);                                                         // find the region of connected point and ~equal angle
                if( reg.Size < min_reg_size ) continue;                                             // reject small regions 
                reg.toRect(prec, p);                                                                // construct rectangular approximation for the region 
                if (!reg.refine(prec, p, density_th)) continue;
                log_nfa = reg.rectImprove(logNT, log_eps);                                          // compute NFA value 
                if( log_nfa <= log_eps ) continue;
                // A New Line Segment was found! 
                reg.rec.move(0.5,0.5);
                reg.rec.zoom(scale);
                rect re = reg.rec;
                outList.Add7(re.x1, re.y1, re.x2, re.y2, re.width, re.p, log_nfa );                 // add line segment found to output 
                reg.markCurrentRegion2Image(++ls_count);
            }
            // return the result
            if (imgReg!=null) 
            {
                if( reg.RegionImage == null ) error("'region' should be a valid image.");
                imgReg=reg.RegionImage;
            }
            if( outList.size > (uint) int.MaxValue ) error("too many detections to fit in an INT.");
            return outList;
        }

        // LSD Simple Interface with Scale and Region output.
        public static doubleTupleList lsd_scale_region( image<double> img,double scale,ref image<int> imgReg)
        {   // LSD parameters 
            double sigma_scale = 0.6; // Sigma for Gaussian filter is computed as
                                      //  sigma = sigma_scale/scale.                  
            double quant = 2.0;       // Bound to the quantization error on the gradient norm.    
            double ang_th = 22.5;     // Gradient angle tolerance in degrees.          
            double log_eps = 0.0;     // Detection threshold: -log10(NFA) > log_eps   
            double density_th = 0.7;  // Minimal density of region points in rectangle.
            int n_bins = 1024;        // Number of bins in pseudo-ordering of gradient modulus.
            return LineSegmentDetection(img, scale, sigma_scale, quant,
                                        ang_th, log_eps, density_th, n_bins,
                                        ref imgReg);
        }

        // LSD Simple Interface with Scale.
        doubleTupleList lsd_scale(image<double> img,double scale)
        {
            image<int> imgReg = null;
            return lsd_scale_region(img,scale,ref imgReg);
        }

        // LSD Simple Interface.
        doubleTupleList lsd(image<double> img)
        {   // LSD parameters 
            double scale = 0.8;       // Scale the image by Gaussian filter to 'scale'.
            return lsd_scale(img,scale);
        }
        #endregion ------ LSD ------
    }  // End of class LSD
}
