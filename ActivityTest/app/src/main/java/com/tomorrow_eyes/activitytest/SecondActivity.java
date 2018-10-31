package com.tomorrow_eyes.activitytest;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.TextView;

public class SecondActivity extends BaseActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.second_layout);
        Intent intent = getIntent();
        String data = intent.getStringExtra("extra_data");
        TextView tv = (TextView)findViewById(R.id.textViewMsg);
        tv.setText(data);
    }

    public void onClickToSee(View v) {
        Intent intent = new Intent();
        intent.putExtra("data_return", getString(R.string.to_see));
        setResult(RESULT_OK,intent);
        finish();
    }

    public void onClickNoSee(View v) {
        Intent intent = new Intent();
        intent.putExtra("data_return", getString(R.string.no_see));
        setResult(RESULT_OK,intent);
        finish();
    }

    @Override
    public void onBackPressed() {
        Intent intent = new Intent();
        intent.putExtra("data_return", "她不說話");
        setResult(RESULT_OK,intent);
        finish();
    }
}
