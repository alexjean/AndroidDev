package com.tomorrow_eyes.preferences;

import android.content.SharedPreferences;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.View;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.Toast;

import java.util.ArrayList;
import java.util.List;

public class MainActivity extends AppCompatActivity {

    RecyclerView recyclerView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        recyclerView = (RecyclerView)findViewById(R.id.msgView);
        LinearLayoutManager layoutManager = new LinearLayoutManager(this);
        layoutManager.setOrientation(LinearLayoutManager.VERTICAL);
        recyclerView.setLayoutManager(layoutManager);
        StringAdapter adapter = new StringAdapter(new ArrayList<String>());
        recyclerView.setAdapter(adapter);
    }

    public SharedPreferences myPreferences() {
        return getSharedPreferences("data",MODE_PRIVATE);
    }

    public void onClickSaveData(View v) {
        SharedPreferences.Editor editor = myPreferences().edit();

        EditText editTextName =(EditText)findViewById(R.id.editTextName);
        EditText editTextInt =(EditText)findViewById(R.id.editTextInt);
        CheckBox checkBoxMarried = (CheckBox)findViewById(R.id.checkBoxMarried);
        editor.putString("name", editTextName.getText().toString());
        String str = editTextInt.getText().toString();
        try {
            int age = Integer.valueOf(str);
            editor.putInt("age", age);
            editor.putBoolean("married", checkBoxMarried.isChecked());
            editor.apply();
        } catch (Exception e) {
            Toast.makeText(this,"ex:"+e.toString(),Toast.LENGTH_SHORT).show();
            e.printStackTrace();
        }
    }

    public void onClickRestoreData(View v) {
        SharedPreferences pref = myPreferences();
        String name = pref.getString("name", "");
        int age = pref.getInt("age", 0);
        boolean married = pref.getBoolean("married", false);
        List<String> list = new ArrayList<String>();
        list.add("");
        list.add("name: " + name);
        list.add("age:    " + Integer.toString(age));
        list.add("married: " + Boolean.toString(married));
        StringAdapter adapter = new StringAdapter(list);
        recyclerView.setAdapter(adapter);
    }
}
