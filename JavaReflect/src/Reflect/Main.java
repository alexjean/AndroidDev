package Reflect;

class Demo {
    int i;
}

public class Main {
    public static void main(String[] args) {
        Class<?> demo1 = null;
        Class<?> demo2 = null;
        Class<?> demo3 = null;
        try {
            demo1 = Class.forName("Reflect.Demo");
        } catch (Exception e) {
            e.printStackTrace();
        }
        demo2 = new Demo().getClass();
        demo3 = Demo.class;

        System.out.println("類名稱 "+demo1.getName());
        System.out.println("類名稱 "+demo2.getName());
        System.out.println("類名稱 "+demo3.getName());

    }
}
