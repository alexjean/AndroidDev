package com.tomorrow_eyes.stopwatch3;

import android.annotation.SuppressLint;
import android.media.MediaPlayer;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.os.Message;
import androidx.fragment.app.FragmentManager;
import androidx.fragment.app.FragmentTransaction;
import androidx.appcompat.app.AppCompatActivity;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.WindowManager;

import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.Timer;
import java.util.TimerTask;

import com.tomorrow_eyes.stopwatch3.SetupFragment.SetSeconds;
import com.tomorrow_eyes.stopwatch3.databinding.ActivityMainBinding;

public class MainActivity extends AppCompatActivity implements SetSeconds {


	Timer timer=null;
	TimerTask task=null;
	int maxCount ;
	int counter;

	public void setSeconds(int seconds) {
		this.maxCount = seconds;
		this.counter = seconds;
		ShowCount();
		WriteToFile(counter);
	}

	MediaPlayer mPlayer;

	private ActivityMainBinding binding;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
//      setContentView(R.layout.activity_main);
        binding = ActivityMainBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        maxCount = ReadFromFile();
        if (maxCount <=0)
        	maxCount = 15;
        counter = maxCount;
        mPlayer=MediaPlayer.create(this,R.raw.heaven);
        try {
			mPlayer.prepare();
		} catch (IllegalStateException | IOException e) {
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
    	String str = String.valueOf(counter);
		// TextView textView=(TextView)findViewById(R.id.textViewSeconds);
		// textView.setText(str);
		binding.textViewSeconds.setText(str);
    }

    private void WriteToFile(int seconds) {
    	String fileName = getExternalFilesDir(null) + "/config.dat";
		FileOutputStream stream;
		try {
			byte[] buf = Integer.toString(seconds).getBytes();
			stream = new FileOutputStream(fileName, false);
			stream.write(buf);
			stream.close();
		} catch (IOException e) {
			e.printStackTrace();
		}
	}

	private int ReadFromFile() {
		String fileName = getExternalFilesDir(null) + "/config.dat";
		FileInputStream stream;
		int i = 0;
		try {
			byte[] buf = new byte[256];
			stream = new FileInputStream(fileName);
			i = stream.read(buf, 0, 250);
			stream.close();
			if (i > 0) {
				String str = new String(buf, 0, i);
				i = Integer.parseInt(str);
			}
		} catch (IOException e) {
			e.printStackTrace();
		}
		return i;
	}

    @SuppressLint("HandlerLeak")
	final Handler handler=new Handler(Looper.getMainLooper()) {
    	public void handleMessage(Message msg){
    		if (msg.what == 49) {
    		     counter--;
    			 ShowCount();
    			 if (counter <= 0) {
                    if (counter > -10)
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
    	counter = maxCount;
    	timer=new Timer(true);
    	newTask();
    	timer.schedule(task, 1000,1000);
    	ShowCount();
    	mPlayer.start();
    }

    public void OnClickReset_Event(View view) {
    	KillTimerAndTask();
    	counter = maxCount;
    	ShowCount();
    }
    
    public void OnClickPause_Event(View view) {
    	KillTimerAndTask();
    }

}
