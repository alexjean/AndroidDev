package com.tomorrow_eyes.jsonobject;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.TextView;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;

public class MainActivity extends AppCompatActivity {

    TextView responseText;
    TextView jsonText;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        responseText = (TextView)findViewById(R.id.response_text);
        jsonText = (TextView)findViewById(R.id.json_text);
    }

    public void onClick(View v) {
        // the URL from PCside 'JsonObjectServer-STS4'
        sendRequestWithURL("http://192.168.88.254:8080/get_data/all");
    }


    private void sendRequestWithURL(final String requestURL) {
        new Thread(new Runnable() {
            @Override
            public void run() {
                HttpURLConnection connection = null;
                BufferedReader reader = null;
                try{
                    URL url = new URL(requestURL);
                    connection = (HttpURLConnection) url.openConnection();
                    connection.setRequestMethod("GET");
                    connection.setConnectTimeout(8000);
                    connection.setReadTimeout(8000);
                    InputStream in = connection.getInputStream();
                    reader = new BufferedReader(new InputStreamReader(in));
                    StringBuilder response = new StringBuilder();
                    String line;
                    while((line=reader.readLine())!=null){
                        response.append(line);
                    }
                    String jsonString = response.toString();
                    showResponse(jsonString);
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        }).start();
    }

    private void showResponse(final String response) {
        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                responseText.setText(response);
                try {
                    JSONArray jsonArray = new JSONArray(response);
                    StringBuilder sb = new StringBuilder();
                    for(int i=0;i < jsonArray.length();i++) {
                        JSONObject jsonObject  = jsonArray.getJSONObject(i);
                        String id = jsonObject.getString("id");
                        String name = jsonObject.getString("name");
                        String version = jsonObject.getString("version");
                        String msg = "id<" + id +"> ver=" + version+ ", name="+ name;
                        Log.d("MainActivity", msg);
                        sb.append(msg); sb.append("\r\n");
                    }
                jsonText.setText(sb);
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        });
    }

    private void parseJsonWithJsonObject(final String jsonData) {
        runOnUiThread(new Runnable() {
            @Override
            public void run() {
            }
        });
    }
}
