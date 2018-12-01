package com.tomorrow_eyes.playvideo;

import android.Manifest;
import android.content.pm.PackageManager;
import android.os.Environment;
import android.support.annotation.NonNull;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Toast;
import android.widget.VideoView;

import java.io.File;

public class MainActivity extends AppCompatActivity {

    private VideoView videoView;
    private static final String writePermission =Manifest.permission.WRITE_EXTERNAL_STORAGE;
    private static final int permissionGranted = PackageManager.PERMISSION_GRANTED;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        videoView = (VideoView)findViewById(R.id.video_view);
        if (ContextCompat.checkSelfPermission(MainActivity.this, writePermission)
                != permissionGranted) {
            ActivityCompat.requestPermissions(MainActivity.this,
                    new String[] {writePermission}, 1);
        } else
            initVideoPath();
    }

    private void initVideoPath() {
        try {
            File sdCard = Environment.getExternalStorageDirectory();
            File file = new File(sdCard ,"Movies/DouYin.mp4");
            videoView.setVideoPath(file.getPath());
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        if (requestCode==1) {
            if (grantResults.length>0 && grantResults[0] == permissionGranted)
                initVideoPath();
            else {
                Toast.makeText(this,"拒絶權限將無法使用本程序",Toast.LENGTH_SHORT).show();
                finish();
            }
        }
    }

    public void onClickPlay(View v) {
        if (!videoView.isPlaying())
            videoView.start();
    }

    public void onClickPause(View v) {
        if (videoView.isPlaying())
            videoView.pause();
    }

    public void onClickReplay(View v) {
        if (videoView.isPlaying()) {
            videoView.resume();
        }
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        if (videoView!=null)
            videoView.suspend();
    }
}



