package com.tomorrow_eyes.camera;

import android.app.Service;
import android.content.Intent;
import android.os.Handler;
import android.os.IBinder;
import android.util.Log;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;

/**
 * Created by Alex on 2016/6/16.
 */
public class ConnectService extends Service {

    public static final String TAG = "chl";
    public static Boolean mainThreadFlag = true;
    public static Boolean ioThreadFlag = true;
    public static final int mBufSize=8192000;
    public static final byte[] mBytes=new byte[mBufSize];
    public static int mRemaining=0;
    public static Handler mUIHandler;
    public static Camera2BasicFragment mCaller;

    ServerSocket serverSocket = null;
    final int SERVER_PORT = 10086;

    @Override
    public IBinder onBind(Intent intent)
    {
        Log.d(TAG, "androidService--->onBind()");
        return null;
    }

    @Override
    public void onCreate()
    {
        super.onCreate();
        Log.d(TAG, "androidService--->onCreate()");
    }

    @Override
    public int onStartCommand(Intent intent, int flags, int startId)
    {
        Log.d(TAG, "androidService--->onStartCommand()");
        mainThreadFlag = true;
        new Thread()
        {
            public void run()
            {
                doListen();
            };
        }.start();
        return START_NOT_STICKY;
    }

    private void doListen()
    {
        serverSocket = null;
        try
        {
            serverSocket = new ServerSocket(SERVER_PORT);
            while (mainThreadFlag)
            {
                Socket socket = serverSocket.accept();
                if (socket!=null) new Thread(new SocketReadWrite(this, socket)).start();
            }
        } catch (IOException e)
        {
            e.printStackTrace();
        }
    }

    @Override
    public void onDestroy()
    {   Log.d(TAG, "androidService--->onDestory()");
        super.onDestroy();
        mainThreadFlag = false;
        ioThreadFlag = false;
        try {
            if (serverSocket != null)
                serverSocket.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}