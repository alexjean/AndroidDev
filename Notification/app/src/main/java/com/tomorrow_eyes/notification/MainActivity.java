package com.tomorrow_eyes.notification;

import android.app.Notification;
import android.app.NotificationManager;
import android.app.NotificationChannel;
import android.app.PendingIntent;
import android.content.Intent;
import android.graphics.BitmapFactory;
import android.graphics.Color;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

    }

    private NotificationChannel createNotificationChannel(String id, String name, String description, int importance, String group_id) {
        NotificationChannel channel = new NotificationChannel(id, name,importance);
        channel.setDescription(description);       // 渠道的描述
        channel.enableLights(true);                // 闪灯
        channel.setLightColor(Color.RED);
        channel.enableVibration(true);             //  震动
        channel.setVibrationPattern(new long[]{10, 500, 500, 500});
        if(group_id!=null)  channel.setGroup(group_id); //设置渠道组
        return channel;
    }

    public void onClickSendNotice(View v) {
        Intent intent= new Intent(this, NotificationActivity.class);
        PendingIntent pi = PendingIntent.getActivity(this,0,intent,0);
        NotificationManager manager = (NotificationManager) getSystemService(NOTIFICATION_SERVICE);
        String channelId = "AlexNotify";
        NotificationChannel channel = createNotificationChannel(channelId ,"LordAlex","",
                NotificationManager.IMPORTANCE_MAX ,null);
        manager.createNotificationChannel(channel);
        Notification notification = new Notification.Builder(this,channelId)
                .setContentTitle("This is content title.")
                .setContentText("This is content text")
                .setWhen(System.currentTimeMillis())
                .setSmallIcon(R.mipmap.ic_launcher_round)
                .setLargeIcon(BitmapFactory.decodeResource(getResources(),R.mipmap.ic_launcher_round))
                .setContentIntent(pi)
                .setAutoCancel(true)
                .build();
        manager.notify(1,notification);

    }
}
