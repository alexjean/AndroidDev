package com.tomorrow_eyes.activitytest;

import android.content.Intent;
import android.os.Bundle;
import android.widget.TextView;

public class ThirdActivity extends BaseActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.third_layout);
        Intent intent = getIntent();
        String url = intent.getDataString();
        TextView tv = (TextView)findViewById(R.id.textViewUrl);
        tv.setText(url);
    }
}
