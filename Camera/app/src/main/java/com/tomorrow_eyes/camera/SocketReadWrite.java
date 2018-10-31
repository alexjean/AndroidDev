package com.tomorrow_eyes.camera;

import android.content.Context;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;

// Created by Alex on 2016/6/16.
public class SocketReadWrite implements Runnable {

    private Socket client;
    private Context context;
    public Boolean bufAvailable=false;
    public int bufPoint=0;

    public SocketReadWrite(Context context, Socket client)
    {
        this.client = client;
        this.context = context;
    }

    private  byte[] createHeader(int remain) {
        byte[] buf=new byte[8];
        buf[0]='J';
        buf[1]='p';
        buf[2]='g';
        for(int i=0;i<3;i++) {
            buf[i+3]=(byte)(remain%256);
            remain/=256;
        }
        byte xor=88;
        for(int i=0;i<7;i++) xor^=buf[i];
        buf[7]=xor;
        return buf;
    }

    @Override
    public void run(){

        OutputStream out;
        InputStream inStream;
        byte[] inBuf=new byte[1024];
        byte[] cmd= {(byte)0xd5,(byte)0xaa,(byte)0x96};
        int inLen=0;
        try {

            out=client.getOutputStream();
            inStream=client.getInputStream();
            ConnectService.ioThreadFlag = true;
            while (ConnectService.ioThreadFlag){
                if(!client.isConnected())
                    break;
                if (inStream.available()>0) {
                    inLen=inStream.read(inBuf);
                    if (inLen>0)
                        for(int i=0;i<inLen-2;i++) {
                            if (inBuf[i]==cmd[0] && inBuf[i+1]==cmd[1] && inBuf[i+2]==cmd[2]) {
                               ConnectService.mUIHandler.post(ConnectService.mCaller.actionTakePicture);
                            }
                        }
                }
                if (ConnectService.mRemaining>0)
                {
                    int remain=ConnectService.mRemaining;
                    if(remain>0) {
                        byte[] header=createHeader(remain);
                        out.write(header);
                        out.flush();
                        out.write(ConnectService.mBytes, 0, remain);
                        out.flush();
                    }
                    ConnectService.mRemaining=0;
                }
            }
            out.close();
        }
        catch (Exception e) { e.printStackTrace(); }
        finally
        {
            try
            {
                if (client != null)
                {
                    client.close();
                }
            } catch (IOException e) { e.printStackTrace(); }
        }
    }
}

