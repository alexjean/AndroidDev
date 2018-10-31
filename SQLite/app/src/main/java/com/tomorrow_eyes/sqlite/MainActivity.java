package com.tomorrow_eyes.sqlite;

import android.content.ContentValues;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;


public class MainActivity extends AppCompatActivity {

    MyDatabaseHelper dbHelper;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        dbHelper = new MyDatabaseHelper(this,"BookStore.db", null,3);
        Button createDatabase = (Button)findViewById(R.id.create_database);
        createDatabase.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                dbHelper.getWritableDatabase();
            }
        });
    }

    public void onClickAddData(View v) {
        SQLiteDatabase db = dbHelper.getWritableDatabase();
        ContentValues values = new ContentValues();
        values.put("name", "The Da Vinci Code");
        values.put("author", "Dan Brown");
        values.put("pages", 454);
        values.put("price", 16.96);
        db.insert("Book",null, values);
/*      values.clear();
        values.put("name", "The Lost Symbol");
        values.put("author", "Dan Brown");
        values.put("pages", 510);
        values.put("price", 19.95);
        db.insert("Book",null, values);
*/
        db.execSQL("insert into book (name,author,pages,price) values(?,?,?,?)",
                new String[] {"The LostSymbol","Dan Brown","510","19.95"});
        Toast.makeText(this, "two books inserted!",Toast.LENGTH_SHORT).show();
    }

    public void onClickQueryData(View v) {
        SQLiteDatabase db = dbHelper.getReadableDatabase();
        Cursor cursor = db.rawQuery("select * from book", null);
        int i=0;
        if (cursor.moveToFirst()) {
            int columnCount = cursor.getColumnCount();
            for (; cursor.moveToNext(); i++) {
                String name = cursor.getString(cursor.getColumnIndex("name"));
                String author = cursor.getString(cursor.getColumnIndex("author"));
                int pages = cursor.getInt(cursor.getColumnIndex("pages"));
                double price = cursor.getDouble(cursor.getColumnIndex("price"));
                Log.d("SQLiteAlex",name+" | \t"+author+"  "+
                        Integer.toString(pages)+" "+Double.toString(price));
            }
        }
        cursor.close();
        Toast.makeText(this, String.format("QueryData pressed! Total %d lines", i ),
                Toast.LENGTH_SHORT).show();
    }

    public void onClickUpdateData(View v) {
        SQLiteDatabase db = dbHelper.getWritableDatabase();
        db.execSQL("update book set price = ? where name = ?",
                new String[] {"10.99","The Da Vinci Code"});
    }

    public void onClickDeleteData(View v) {
        SQLiteDatabase db = dbHelper.getWritableDatabase();
        db.delete("book", "pages > ?", new String [] {"500"});
    }

}
