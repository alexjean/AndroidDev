using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace PC_ADB_USB
{
    struct rect
    {
        public int x,y;
        public int height,width;
    }

    struct Scalar
    {
        double val0, val1, val2, val3;
    }

    class MS
    {
        public class Criteria
        {
            public double epsilon;
            public int max_iter;
        }

        static void error(string msg)
        {
            MessageBox.Show("MS Error:"+msg);
            Application.Exit();
        }

        
        public ColorPoint MeanShift(image<MyColor> imgProb, rect windowIn, Criteria criteria,ref image<Point> comp)
        {
            int eps;
            rect cur_rect = windowIn;
            //if( comp ) comp->rect = windowIn;
            if( windowIn.height <= 0 || windowIn.width <= 0 ) error( "Input window has non-positive sizes" );
            if (windowIn.x < 0 || windowIn.x + windowIn.width > imgProb.width ||
                windowIn.y < 0 || windowIn.y + windowIn.height > imgProb.height)
                error("Initial window is not inside the image ROI");
            uint width = (uint)windowIn.width;
            uint height = (uint)windowIn.height;
            image<double> kernel = image<double>.newImage(width, height);
            double xc = (double)width / 2;
            double yc = (double)height / 2;
            double r  = (width > height) ? height : width;
            double dist,xd,yd;
            for(uint x=0;x<width;x++)
                for (uint y = 0; y < height; y++)
                {
                    xd = Math.Abs(x - xc);  
                    yd = Math.Abs(y - yc);
                    dist = Math.Sqrt(xd * xd + yd * yd)/r;
                    if (dist >= 1) { kernel.data[x, y] = 0; continue; }
                    double Epanechikov = 3 / 4 * (1 - dist * dist);
                    kernel.data[x, y] = Epanechikov;
                }
            eps =(int)Math.Round( criteria.epsilon * criteria.epsilon );
/*
            double m00 = 0,m10 = 0, m01 = 0; // 0阶矩 1阶矩

            for(int  iter = 0; iter < criteria.max_iter; iter++ )    
            {
                int dx, dy, nx, ny;
                double inv_m00;
                for(int y = 0; y < height ; y++)
                {
                    for(int x =0 ; x < width ; x++)
                    {
                        if( ((cur_rect.y + y) < imgProb.height)  &&  ((cur_rect.x + x) < imgProb.width) )
                        {
                            MyColor tmp;
                            tmp = imgProb.data[ windowIn.x + x, windowIn.y + y ];    
                            m00 = m00 + tmp;
                            m10 = m10 + (x+1) * tmp;
                            m01 = m01 + (y+1) * tmp;
                        }
                    }
                }
                // Calculating center of mass 
                if( Math.Abs(m00) < double.Epsilon) break;
                if(m00!=0) 
                        inv_m00 = (1/Math.Sqrt(m00))*(1/Math.Sqrt(m00));         //m00不为0
                else inv_m00 = 0;
                // 计算重心偏移量
                dx = (int)Math.Round( m10 * inv_m00 - (windowIn.width*0.5 +0.5));   //m10/m00  
                dy = (int)Math.Round( m01 * inv_m00 - (windowIn.height*0.5 +0.5));  //m01/m00
                nx = cur_rect.x + dx;                                               //重心位移
                ny = cur_rect.y + dy;

                if      ( nx < 0 ) nx = 0;
                else if ( nx + cur_rect.width > imgProb.width )   nx = (int)imgProb.width - cur_rect.width;

                if      ( ny < 0 )  ny = 0;
                else if ( ny + cur_rect.height > imgProb.height ) ny = (int)imgProb.height - cur_rect.height;
                dx = nx - cur_rect.x;
                dy = ny - cur_rect.y;
                cur_rect.x = nx;                          //更新搜索框位置
                cur_rect.y = ny;
                if (dx * dx + dy * dy < eps) break;       // Check for coverage centers mass & window 
            }
            //if( comp )
            //{
            //    comp->rect = cur_rect;
            //    comp->area = (float)m00;
            //}
*/
            return null;
        }
    }
}
