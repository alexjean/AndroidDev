package com.tomorrow_eyes.databaseprovider;

import android.content.ContentValues;
import android.database.Cursor;
import android.net.Uri;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;

public class MainActivity extends AppCompatActivity {

    private String newId;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }

    public void onClickAdd(View v) {
        Uri uri = Uri.parse("content://com.tomorrow_eyes.databaseprovider.provider/book");
        ContentValues  values = new ContentValues();
        values.put("name","A Clash of Kings");
        values.put("author","George Martin");
        values.put("pages",1040);
        values.put("price",22.85);
        Uri newUri = getContentResolver().insert(uri,values);
        newId = newUri.getPathSegments().get(1);
    }

    private static final String TAG="AlexJean";
    public void onClickQuery(View v) {
        Uri uri = Uri.parse("content://com.tomorrow_eyes.databaseprovider.provider/book");
        Cursor cursor = getContentResolver().query(uri,null,null,null,null);
        if (cursor!=null) {
            while(cursor.moveToNext()) {
                String name = cursor.getString(cursor.getColumnIndex("name"));
                String author = cursor.getString(cursor.getColumnIndex("author"));
                int pages = cursor.getInt(cursor.getColumnIndex("pages"));
                double price= cursor.getDouble(cursor.getColumnIndex("price"));
                Log.d(TAG,"book name='"+name+"' author='"+author+"'");
                Log.d(TAG,"pages "+pages +"   price "+price);
            }
            cursor.close();
        }
    }

    public void onClickUpdate(View v) {
        Uri uri = Uri.parse("content://com.tomorrow_eyes.databaseprovider.provider/book/"+newId);
        ContentValues values = new ContentValues();
        values.put("name","A Storm of Swords");
        values.put("pages",1216);
        values.put("price",24.05);
        getContentResolver().update(uri,values,null,null);
    }

    public void onClickDelete(View v) {
        Uri uri = Uri.parse("content://com.tomorrow_eyes.databaseprovider.provider/book/"+newId);
        getContentResolver().delete(uri,null,null);
    }


}
