package com.tomorrow_eyes.stopwatch3;

import android.annotation.SuppressLint;
import android.media.MediaPlayer;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;
import android.support.v7.app.AppCompatActivity;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.WindowManager;

import java.io.IOException;
import java.util.Timer;
import java.util.TimerTask;

import com.tomorrow_eyes.stopwatch3.SetupFragment.SetSeconds;
import com.tomorrow_eyes.stopwatch3.databinding.ActivityMainBinding;

public class MainActivity extends AppCompatActivity implements SetSeconds {


	Timer timer=null;
	TimerTask task=null;
	int maxCount = 15;
	int countSeconds = maxCount;

	public void setSeconds(int seconds) {
		this.maxCount = seconds;
		this.countSeconds = seconds;
		ShowCount();
	}

	MediaPlayer mPlayer;

	private ActivityMainBinding binding;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
//      setContentView(R.layout.activity_main);
        binding = ActivityMainBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

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
	protected void onStart() {
		super.onStart();
		ShowCount();
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
				addFragment();
				//Toast.makeText(this,"設定尚未完工",Toast.LENGTH_SHORT).show();
		}
		return true;
	}

	private void addFragment() {
    	final String tag="SetupFragment";
    	FragmentManager fragManager = getSupportFragmentManager();
		if (fragManager.findFragmentByTag(tag) == null) {
			FragmentTransaction transaction = fragManager.beginTransaction();
			transaction.add(R.id.setupContainer, SetupFragment.newInstance(maxCount), tag);
			transaction.addToBackStack(null);
			transaction.commit();
		}
	}

	private void ShowCount(){
    	String str = String.valueOf(countSeconds);
    	// TextView textView=(TextView)findViewById(R.id.textViewSeconds);
    	// textView.setText(str);
		binding.textViewSeconds.setText(str);
    }

    @SuppressLint("HandlerLeak")
	final Handler handler=new Handler() {
    	public void handleMessage(Message msg){
    		if (msg.what == 49) {
    		     countSeconds--;
    			 ShowCount();
    			 if (countSeconds <= 0) {
                    if (countSeconds > -10)
                        mPlayer.start();
                    else
                        KillTimerAndTask();
                 }
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
    	countSeconds = maxCount;
    	timer=new Timer(true);
    	newTask();
    	timer.schedule(task, 1000,1000);
    	ShowCount();
    	mPlayer.start();
    }

    public void OnClickReset_Event(View view) {
    	KillTimerAndTask();
    	countSeconds = maxCount;
    	ShowCount();
    }
    
    public void OnClickPause_Event(View view) {
    	KillTimerAndTask();
    }

}
