package com.tomorrow_eyes.broadcast2;

import android.content.IntentFilter;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;

public class MainActivity extends AppCompatActivity {

    private IntentFilter intentFilter;
    private AnotherBroadcastReceiver mReceiver;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        intentFilter = new IntentFilter();
        intentFilter.addAction("com.tomorrow_eyes.broadcast.MY_BROADCAST");
        intentFilter.setPriority(90);  // 測試priority好像沒用, 999最高
        mReceiver = new AnotherBroadcastReceiver();
        registerReceiver(mReceiver,intentFilter);
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        unregisterReceiver(mReceiver);
    }
}
