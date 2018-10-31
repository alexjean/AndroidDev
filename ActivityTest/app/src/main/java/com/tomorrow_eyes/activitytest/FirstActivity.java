package com.tomorrow_eyes.activitytest;

import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

public class FirstActivity extends BaseActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.first_layout);
        Button btnExit = (Button)findViewById(R.id.btnExit);
        btnExit.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                //Intent intent = new Intent(FirstActivity.this, FirstActivity.class);
                //startActivity(intent);
                finish();
            }
        });
    }

    public void onClickButton1(View v) {
        // Toast.makeText(FirstActivity.this,"You clicked Own Button1 Listener",Toast.LENGTH_SHORT).show();
        String data = "再見亦是枉然";
        Intent intent = new Intent(FirstActivity.this, SecondActivity.class);  // 顯式
        intent.putExtra("extra_data", data);
        startActivityForResult(intent,1);

        //Intent intent = new Intent("com.tomorrow_eyes.activitytest.ACTION_START"); // 隱式,要加intent-filter
        //intent.addCategory("com.tomorrow_eyes.activitytest.MY_CATEGORY");
        //startActivity(intent);
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        switch (requestCode){
            case 1:
                if (resultCode == RESULT_OK) {
                    String returnedData = data.getStringExtra("data_return");
                    TextView tv = (TextView)findViewById(R.id.textViewAnswer);
                    tv.setText(returnedData);
                }
                break;
            default:
        }
    }

    public void onClickBtnDial10086(View v) {
        Intent intent = new Intent(Intent.ACTION_DIAL);
        intent.setData(Uri.parse("tel:10086"));
        startActivity(intent);
    }

    public void onClickBtnBaidu(View v) {
        Intent intent = new Intent(Intent.ACTION_VIEW);
        intent.setData(Uri.parse("http://www.baidu.com"));
        startActivity(intent);
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.main,menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch(item.getItemId()) {
            case R.id.add_item:
                Toast.makeText(this,"你按了 Add",Toast.LENGTH_SHORT).show();
                break;
            case R.id.remove_item:
                Toast.makeText(this,"你按了 Remove",Toast.LENGTH_SHORT).show();
                break;
            default:
        }
        return true;
    }
}
