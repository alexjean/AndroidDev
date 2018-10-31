package com.tomorrow_eyes.makecall;

import android.Manifest;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.net.Uri;
import android.support.annotation.NonNull;
import android.support.v4.content.ContextCompat;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Toast;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }

    final public String permCallPhone = Manifest.permission.CALL_PHONE;
    final public int requestCallPhone = 1;
    public void onClickMakeCall(View v) {
        if (checkSelfPermission(permCallPhone) != PackageManager.PERMISSION_GRANTED)
            requestPermissions(new String[] { permCallPhone } ,requestCallPhone);
        else call();
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        switch (requestCode) {
            case requestCallPhone:
                if (grantResults.length>0 && grantResults[0]==PackageManager.PERMISSION_GRANTED)
                    call();
                else
                    Toast.makeText(this,"You denied the permission to Call!",
                            Toast.LENGTH_SHORT).show();
                break;
            default:
                super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }

    public void call()
    {
        try {
            Intent intent = new Intent(Intent.ACTION_CALL);
            intent.setData(Uri.parse("tel:18618150828"));
            startActivity(intent);
        } catch (SecurityException e) {
            Log.d("AlexJean", e.toString());
            e.printStackTrace();
        }
    }
}
