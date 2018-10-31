package com.tomorrow_eyes.stopwatch3;

import android.annotation.SuppressLint;
import android.media.MediaPlayer;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.support.v7.app.AppCompatActivity;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.WindowManager;
import android.widget.TextView;
import android.widget.Toast;

import java.io.IOException;
import java.util.Timer;
import java.util.TimerTask;

public class MainActivity extends AppCompatActivity {

	Timer timer=null;
	TimerTask task=null;
	int count=0;
	MediaPlayer mPlayer;
	
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        mPlayer=MediaPlayer.create(this,R.raw.heaven);
        try {
			mPlayer.prepare();
		} catch (IllegalStateException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		}
        getWindow().addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
    }
    
    @Override
    protected void onPause( ) {
    	KillTimerAndTask();
    	super.onPause();
    }


    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.main, menu);
        return true;
    }

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
    	switch(item.getItemId()) {
			case R.id.itemLeave:
				finish();
				break;
			case R.id.actionSettings:
				Toast.makeText(this,"設定尚未完工",Toast.LENGTH_SHORT).show();
		}
		return true;
	}

	private void ShowCount(){
    	TextView textView=(TextView)findViewById(R.id.textViewSeconds);
    	textView.setText(String.valueOf(count));
    }

    @SuppressLint("HandlerLeak")
	final Handler handler=new Handler() {
    	public void handleMessage(Message msg){
    		switch (msg.what) {
    		case 49: count++;
    				 ShowCount();
    				 if ((count%30)==0) {
    					 mPlayer.start();
    				 }
    				 break;
    		}
    	}
    };

    void newTask() {
    	task=new TimerTask() {
		    	public void run() {
		    		Message message=new Message();
		    		message.what=49;
		    		handler.sendMessage(message);
		    	}
	    };
    }
    
    void KillTimerAndTask() {
    	if (timer==null) return;
    	timer.cancel();
    	timer.purge();
    	timer=null;
    	if (task!=null) {
    		task.cancel();
    		task=null;
    	}
    }
    
    public void OnClickStart_Event(View view) {
    	if (timer!=null) {
    		return;
    	}
    	timer=new Timer(true);
    	newTask();
    	timer.schedule(task, 1000,1000);
    	ShowCount();
    	mPlayer.start();
    }

    public void OnClickReset_Event(View view) {
    	KillTimerAndTask();
    	count=0;
    	ShowCount();
    }
    
    public void OnClickPause_Event(View view) {
    	KillTimerAndTask();
    }

}
