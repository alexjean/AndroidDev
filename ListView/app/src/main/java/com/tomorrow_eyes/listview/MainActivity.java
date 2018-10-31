package com.tomorrow_eyes.listview;

import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.Toast;

import java.util.ArrayList;
import java.util.List;

public class MainActivity extends AppCompatActivity {
    private String[] data = {"Apple", "Banana", "Orange", "Watermelon",
            "Pear", "Grape", "Pineapple", "Strawberry", "Cherry", "Mango"};
    private List<Fruit> fruitList = new ArrayList<Fruit>();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        initFruits("");
        for(int i=0;i<10;i++)
            initFruits(Integer.toString(i));

        //ArrayAdapter<String> adapter = new ArrayAdapter<String>(MainActivity.this, android.R.layout.simple_list_item_1, data);
        FruitAdapter adapter = new FruitAdapter(MainActivity.this, R.layout.fruit_item, fruitList);
        ListView listView = findViewById(R.id.list_view);
        listView.setAdapter(adapter);
        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int position, long l) {
                Fruit fruit = fruitList.get(position);
                Toast.makeText(MainActivity.this,
                        fruit.getName(),Toast.LENGTH_SHORT).show();
            }
        });
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
