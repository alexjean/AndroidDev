package com.tomorrow_eyes.recyclerview;

import android.support.annotation.NonNull;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.StaggeredGridLayoutManager;

import java.util.ArrayList;
import java.util.List;

public class MainActivity extends AppCompatActivity {

    private List<Fruit> fruitList = new ArrayList<Fruit>();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        initFruits("");
        for(int i=0;i<10;i++)
            initFruits(Integer.toString(i));

        RecyclerView recyclerView =(RecyclerView) findViewById(R.id.recycler_view);
//        LinearLayoutManager layoutManager = new LinearLayoutManager(this);
//        layoutManager.setOrientation(LinearLayoutManager.HORIZONTAL);
        StaggeredGridLayoutManager layoutManager =
                new StaggeredGridLayoutManager(3 , StaggeredGridLayoutManager.VERTICAL);
        recyclerView.setLayoutManager(layoutManager);
        FruitAdapter adapter = new FruitAdapter(fruitList);
        recyclerView.setAdapter(adapter);
    }

    private void initFruits(@NonNull String postfix ) {
        fruitList.add(new Fruit("Apple" + postfix, R.drawable.apple_pic));
        fruitList.add(new Fruit("Banana"+ postfix, R.drawable.banana_pic));
        fruitList.add(new Fruit("Orange"+ postfix, R.drawable.orange_pic));
        fruitList.add(new Fruit("Watermelon"+ postfix, R.drawable.watermelon_pic));
        fruitList.add(new Fruit("Pear"+ postfix  , R.drawable.pear_pic));
        fruitList.add(new Fruit("Grape"+ postfix , R.drawable.grape_pic));
        fruitList.add(new Fruit("PineApple"+ postfix , R.drawable.pineapple_pic));
        fruitList.add(new Fruit("Strawberry"+ postfix, R.drawable.strawberry_pic));
        fruitList.add(new Fruit("Cherry"+ postfix, R.drawable.cherry_pic));
        fruitList.add(new Fruit("Mango"+ postfix , R.drawable.mango_pic));
    }
}
