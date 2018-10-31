cvMeanShift( const void* imgProb, CvRect windowIn,
             CvTermCriteria criteria, CvConnectedComp* comp )
{
    CvMoments moments;
    int    i = 0, eps;
    CvMat  stub, *mat = (CvMat*)imgProb;
    CvMat  cur_win;
    CvRect cur_rect = windowIn;

    CV_FUNCNAME( "cvMeanShift" );

    if( comp )
        comp->rect = windowIn;

    moments.m00 = moments.m10 = moments.m01 = 0;

    __BEGIN__;

    CV_CALL( mat = cvGetMat( mat, &stub ));

    if( CV_MAT_CN( mat->type ) > 1 )
        CV_ERROR( CV_BadNumChannels, cvUnsupportedFormat );

    if( windowIn.height <= 0 || windowIn.width <= 0 )
        CV_ERROR( CV_StsBadArg, "Input window has non-positive sizes" );

    if( windowIn.x < 0 || windowIn.x + windowIn.width > mat->cols ||
        windowIn.y < 0 || windowIn.y + windowIn.height > mat->rows )
        CV_ERROR( CV_StsBadArg, "Initial window is not inside the image ROI" );

    CV_CALL( criteria = cvCheckTermCriteria( criteria, 1., 100 ));

    eps = cvRound( criteria.epsilon * criteria.epsilon );

    for( i = 0; i < criteria.max_iter; i++ )//迭代max_iter次，调用meanshift时传入
    {
        int dx, dy, nx, ny;
        double inv_m00;
  //得到子矩阵（搜索框）
        CV_CALL( cvGetSubRect( mat, &cur_win, cur_rect ));//返回输入的矩阵的矩形数组子集的矩阵头到cur_win
        CV_CALL( cvMoments( &cur_win, &moments ));//计算矩

        /* Calculating center of mass */
        if( fabs(moments.m00) < DBL_EPSILON )
            break;

        inv_m00 = moments.inv_sqrt_m00*moments.inv_sqrt_m00;//m00不为0
        // 计算重心偏移量
        dx = cvRound( moments.m10 * inv_m00 - windowIn.width*0.5 );//m10/m00  
        dy = cvRound( moments.m01 * inv_m00 - windowIn.height*0.5 );//m01/m00

        nx = cur_rect.x + dx;
        ny = cur_rect.y + dy;

        if( nx < 0 )
            nx = 0;
        else if( nx + cur_rect.width > mat->cols )
            nx = mat->cols - cur_rect.width;

        if( ny < 0 )
            ny = 0;
        else if( ny + cur_rect.height > mat->rows )
            ny = mat->rows - cur_rect.height;

        dx = nx - cur_rect.x;
        dy = ny - cur_rect.y;
        cur_rect.x = nx;//更新搜索框位置
        cur_rect.y = ny;

        /* Check for coverage centers mass & window */
        if( dx*dx + dy*dy < eps )
            break;
    }

    __END__;

    if( comp )
    {
        comp->rect = cur_rect;
        comp->area = (float)moments.m00;
    }

    return i;
}


////////////////////////////////// cvCamShift ////////////////////////////////////////////////
 

cvCamShift( const void* imgProb, CvRect windowIn,
            CvTermCriteria criteria,
            CvConnectedComp* _comp,
            CvBox2D* box )
{
    const int TOLERANCE = 10;
    CvMoments moments;
    double m00 = 0, m10, m01, mu20, mu11, mu02, inv_m00;
    double a, b, c, xc, yc;
    double rotate_a, rotate_c;
    double theta = 0, square;
    double cs, sn;
    double length = 0, width = 0;
    int itersUsed = 0;
    CvConnectedComp comp;
    CvMat  cur_win, stub, *mat = (CvMat*)imgProb;

    CV_FUNCNAME( "cvCamShift" );

    comp.rect = windowIn;//初始搜索窗口

    __BEGIN__;

    CV_CALL( mat = cvGetMat( mat, &stub ));//目标图

    CV_CALL( itersUsed = cvMeanShift( mat, windowIn, criteria, &comp ));//调用meanshift
    windowIn = comp.rect;

 //搜索窗口扩大TOLERANCE，
    windowIn.x -= TOLERANCE;
    if( windowIn.x < 0 )
        windowIn.x = 0;

    windowIn.y -= TOLERANCE;
    if( windowIn.y < 0 )
        windowIn.y = 0;

    windowIn.width += 2 * TOLERANCE;
    if( windowIn.x + windowIn.width > mat->width )
        windowIn.width = mat->width - windowIn.x;

    windowIn.height += 2 * TOLERANCE;
    if( windowIn.y + windowIn.height > mat->height )
        windowIn.height = mat->height - windowIn.y;

//返回输入的矩阵的矩形数组子集的矩阵头到cur_win
    CV_CALL( cvGetSubRect( mat, &cur_win, windowIn ));

    /* Calculating moments in new center mass *///计算矩
    CV_CALL( cvMoments( &cur_win, &moments ));

    m00 = moments.m00;//0阶矩
    m10 = moments.m10;
    m01 = moments.m01;
    mu11 = moments.mu11;
    mu20 = moments.mu20;
    mu02 = moments.mu02;

    if( fabs(m00) < DBL_EPSILON )
        EXIT;

    inv_m00 = 1. / m00;
    xc = cvRound( m10 * inv_m00 + windowIn.x );//搜索窗口位置
    yc = cvRound( m01 * inv_m00 + windowIn.y );
    a = mu20 * inv_m00;
    b = mu11 * inv_m00;
    c = mu02 * inv_m00;

    /* Calculating width & height */
    square = sqrt( 4 * b * b + (a - c) * (a - c) );

    /* Calculating orientation */
    theta = atan2( 2 * b, a - c + square );

    /* Calculating width & length of figure */
    cs = cos( theta );
    sn = sin( theta );

    rotate_a = cs * cs * mu20 + 2 * cs * sn * mu11 + sn * sn * mu02;
    rotate_c = sn * sn * mu20 - 2 * cs * sn * mu11 + cs * cs * mu02;
    length = sqrt( rotate_a * inv_m00 ) * 4;//长
    width = sqrt( rotate_c * inv_m00 ) * 4;//宽

    /* In case, when tetta is 0 or 1.57... the Length & Width may be exchanged */
    if( length < width )
    {
        double t;
        
        CV_SWAP( length, width, t );
        CV_SWAP( cs, sn, t );
        theta = CV_PI*0.5 - theta;
    }

    /* Saving results */
    if( _comp || box )
    {
        int t0, t1;
        int _xc = cvRound( xc );
        int _yc = cvRound( yc );

        t0 = cvRound( fabs( length * cs ));
        t1 = cvRound( fabs( width * sn ));

        t0 = MAX( t0, t1 ) + 2;
        comp.rect.width = MIN( t0, (mat->width - _xc) * 2 );

        t0 = cvRound( fabs( length * sn ));
        t1 = cvRound( fabs( width * cs ));

        t0 = MAX( t0, t1 ) + 2;
        comp.rect.height = MIN( t0, (mat->height - _yc) * 2 );

        comp.rect.x = MAX( 0, _xc - comp.rect.width / 2 );
        comp.rect.y = MAX( 0, _yc - comp.rect.height / 2 );

        comp.rect.width = MIN( mat->width - comp.rect.x, comp.rect.width );
        comp.rect.height = MIN( mat->height - comp.rect.y, comp.rect.height );
        comp.area = (float) m00;
    }

    __END__;

    if( _comp )
        *_comp = comp;
    
    if( box )
    {
        box->size.height = (float)length;
        box->size.width = (float)width;
        box->angle = (float)(theta*180./CV_PI);
        box->center = cvPoint2D32f( comp.rect.x + comp.rect.width*0.5f,
                                    comp.rect.y + comp.rect.height*0.5f);
    }

    return itersUsed;
}

 

 

参考文献：

《学习opencv》中文版   

Computer Vision Face Tracking For Use in a Perceptual User Interface    。Gary R. Bradski  1998

cvhistogram.cpp   cvcamshift.cpp

**************************************************************** 给力的分割线  ***********************************************************************

 我的论文程序里用的meanshift函数，从opencv的meanshift函数改动而来（自己写的代码来计算0阶矩、1阶矩，其他小改动了哈，使之适应小数据集的均值飘逸计算）

CV_IMPL int myMeanShift( const IplImage* imgProb, CvRect windowIn, CvTermCriteria criteria, CvConnectedComp* comp )
{
       int  i = 0, eps;
    CvRect cur_rect = windowIn;

    CV_FUNCNAME( "myMeanShift" );

    if( comp )
        comp->rect = windowIn;

    double m00 = 0; 
 double m10 = 0; 
 double m01 = 0;//0阶矩 1阶矩

    __BEGIN__;

 

    if( windowIn.height <= 0 || windowIn.width <= 0 )
        CV_ERROR( CV_StsBadArg, "Input window has non-positive sizes" );

    if( windowIn.x < 0 || windowIn.x + windowIn.width > imgProb->width ||
        windowIn.y < 0 || windowIn.y + windowIn.height > imgProb->height )
        CV_ERROR( CV_StsBadArg, "Initial window is not inside the image ROI" );

    CV_CALL( criteria = cvCheckTermCriteria( criteria, 1., 100 ));

    eps = cvRound( criteria.epsilon * criteria.epsilon );

    for( i = 0; i < 1/*criteria.max_iter*/; i++ )//迭代100次
    {
        int dx, dy, nx, ny;
        double inv_m00;
  
  //////////////////////////计算矩 m00 m10 m01////////////////////////////////////////////////

  //得到图像的值 , cvGet2D(img,行,列)
  cout<<"val  ";
  for(int i = 0; i < windowIn.height ; i++)
  {
   for(int j =0 ; j < windowIn.width ; j++)
   {
    if(  ((cur_rect.y + i) < imgProb->height)  &&  ((cur_rect.x + j) < imgProb->width)  )
    {
     CvScalar tmp;
     tmp = cvGet2D(imgProb , windowIn.y + i, windowIn.x + j);//行、列

     cout<<" "<<tmp.val[0];

     m00 = m00 + tmp.val[0];

     m10 = m10 + (j+1) * tmp.val[0];
     m01 = m01 + (i+1) * tmp.val[0];

    }

   }

   cout<<endl;
  }
  cout<<endl<<"m00 "<<m00<<" "<<m10<<" "<<m01<<endl;


  //////////////////////////////////////////////////////////////////////////
  


        /* Calculating center of mass */
        if( fabs(m00) < DBL_EPSILON )
            break;

  if(m00!=0)
  {
   inv_m00 = (1/sqrt(m00))*(1/sqrt(m00));//m00不为0
  }
        // 计算重心偏移量
        dx = cvRound( m10 * inv_m00 - (windowIn.width*0.5 +0.5));//m10/m00  
        dy = cvRound( m01 * inv_m00 - (windowIn.height*0.5 +0.5));//m01/m00

        nx = cur_rect.x + dx;//重心位移
        ny = cur_rect.y + dy;

  cout<<"center:"<<m10 * inv_m00<<" "<<m01 * inv_m00<<endl ;
  cout<<"rect.x y "<<cur_rect.x<<" "<<cur_rect.y<<"  nx ny :"<<nx<<" "<<ny<<endl;
  cout<<"dx dy11:"<<m10 * inv_m00 - (windowIn.width*0.5+0.5)<<" "<<m01 * inv_m00 - (windowIn.height*0.5+0.5)<<"   ";
  cout<<"win  width height :"<<windowIn.x<<" "<<windowIn.y<<" "<<windowIn.width*0.5<<"  "<<windowIn.height*0.5<<endl;
  cout<<"dx dy:"<<dx<<" "<<dy<<endl;

        if( nx < 0 )
            nx = 0;
        else if( nx + cur_rect.width > imgProb->width )
            nx = imgProb->width - cur_rect.width;

        if( ny < 0 )
            ny = 0;
        else if( ny + cur_rect.height > imgProb->height )
            ny = imgProb->height - cur_rect.height;

        dx = nx - cur_rect.x;
        dy = ny - cur_rect.y;
        cur_rect.x = nx;//更新搜索框位置
        cur_rect.y = ny;

        /* Check for coverage centers mass & window */
        if( dx*dx + dy*dy < eps )
            break;
    }

    __END__;

    if( comp )
    {
        comp->rect = cur_rect;
        comp->area = (float)m00;
    }

    return i;
}