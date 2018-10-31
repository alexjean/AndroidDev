using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PC_ADB_USB
{
    class MyColor
    {
        double R;
        double G;
        double B;
    }

    class ColorPoint
    {
        double x, y;
        MyColor color;
        //double color2mean;
    }
    //
    //    * * *
    //  * * * * *
    //  * * # * *
    //  * * * * *
    //    * * * 
    // 先查周圍20點, 不計邊緣,如果過7成和自己差距小,發起區域
    // 全圖發起點,同時grow, 邊緣競爭搶點
    // 
    class ColorRegion
    {
        int id;
        List<ColorPoint> list;
        MyColor mean;
        bool Add(ColorPoint p) { return true; }
    }
    class RegionGrow
    {
        image<double> angles;       // 微分角
        Bitmap image;               // 帶色彩
        double tolerance;           // 色彩向量的區容忍值
        int    maxId;
        List<ColorRegion> reg;
        image<int> regMap;
        bool amIBoss(int x, int y) { return true; }
        ColorRegion regionBorn(int x, int y) { return null; }
        bool grow(ColorRegion re) { return true; }    // 一但return false,表示長不了
        ColorRegion vote(ColorPoint p, ColorRegion r1, ColorRegion r2) { return null; }
        List<ColorRegion> Slove(Bitmap original, image<double> angles, double torlerence)
        {
            return null;
        }
    }
}
