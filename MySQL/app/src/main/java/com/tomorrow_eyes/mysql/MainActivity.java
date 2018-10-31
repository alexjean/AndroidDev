package com.tomorrow_eyes.mysql;

import android.Manifest;
import android.content.pm.PackageManager;
import android.support.annotation.NonNull;
import android.support.v4.content.ContextCompat;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;

import java.sql.Connection;
import java.sql.SQLException;

public class MainActivity extends AppCompatActivity {

    private static final String REMOTE_IP="119.23.34.93";
    private static final String URL = "jdbc:mysql://" + REMOTE_IP +"/alex";
    private static final String USER = "alex";
    private static final String PASSWORD = "CalcVoucher888";

    private Connection conn;
    int RequestCode = 0x3306;  // for mysql
    static String TAG = "LoadALex";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        doPermission();
    }

    protected void doPermission() {
        String permission = Manifest.permission.INTERNET;
        String[] permissions = { permission };
        int permissionCheck = ContextCompat.checkSelfPermission(this, permission);
        if (permissionCheck != PackageManager.PERMISSION_GRANTED)
            requestPermissions(permissions, RequestCode);
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        if (RequestCode != requestCode)
            super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        else {
            for(int i=0;i<permissions.length;i++){
                Log.d(TAG,permissions[i]+"=>"+Integer.toString(grantResults[i]));
            }


        }
    }

    public void onConn(View view) {
        conn = Util.openConnection(URL, USER, PASSWORD);
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        if (conn!=null) {
            try { conn.close(); }
            catch (SQLException e) {   conn = null;   }
            finally {  conn = null;   }
        }
    }
}
