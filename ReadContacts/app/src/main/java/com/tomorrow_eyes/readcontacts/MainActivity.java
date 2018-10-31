package com.tomorrow_eyes.readcontacts;

import android.Manifest;
import android.content.pm.PackageManager;
import android.database.Cursor;
import android.provider.ContactsContract.CommonDataKinds.Phone;
import android.support.annotation.NonNull;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.widget.ArrayAdapter;
import android.widget.Toast;

import java.util.ArrayList;
import java.util.List;

public class MainActivity extends AppCompatActivity {

    AlexAdapter adapter;
    List<AlexPhoneContact> contactsList = new ArrayList<AlexPhoneContact>();
    final String permReadContact = Manifest.permission.READ_CONTACTS;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        if (checkSelfPermission(permReadContact)!=PackageManager.PERMISSION_GRANTED)
            requestPermissions(new String[] {permReadContact} ,1);
        else readContacts();

        RecyclerView contactsView = (RecyclerView) findViewById(R.id.contacts_view);
        LinearLayoutManager layoutManager = new LinearLayoutManager(this);
        layoutManager.setOrientation(LinearLayoutManager.VERTICAL);
        contactsView.setLayoutManager(layoutManager);
        adapter = new AlexAdapter(contactsList);
        contactsView.setAdapter(adapter);
    }

    private void readContacts() {

        try (Cursor cursor = getContentResolver().query(
                Phone.CONTENT_URI,null, null, null, null)) {
            if (cursor != null) {
                Integer indexDisplayName = cursor.getColumnIndex(Phone.DISPLAY_NAME);
                Integer indexNumber = cursor.getColumnIndex(Phone.NUMBER);
                while (cursor.moveToNext()) {
                    String displayName = cursor.getString(indexDisplayName);
                    String number = cursor.getString(indexNumber);
                    contactsList.add(new AlexPhoneContact(displayName, number));
                }
                adapter.notifyDataSetChanged();
            }
        } catch (Exception e) {
            Log.d("AlexJean", e.toString());
            e.printStackTrace();
        }
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        switch (requestCode) {
            case 1:
                if (grantResults.length>0 && grantResults[0]== PackageManager.PERMISSION_GRANTED)
                    readContacts();
                else
                    Toast.makeText(this,"You denied the permission",
                            Toast.LENGTH_SHORT).show();
                break;
            default:
                super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}
