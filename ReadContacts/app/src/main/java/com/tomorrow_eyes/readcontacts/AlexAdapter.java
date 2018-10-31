package com.tomorrow_eyes.readcontacts;

import android.support.annotation.NonNull;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.RecyclerView.Adapter;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;


import java.util.List;

public class AlexAdapter extends Adapter<AlexAdapter.ViewHolder> {

    List<AlexPhoneContact> mList;

    static class ViewHolder extends RecyclerView.ViewHolder {
        View contactsView;
        TextView countView;
        TextView nameView;
        TextView numberView;

        public ViewHolder(View v) {
            super(v);
            contactsView = v;
            countView = (TextView) v.findViewById(R.id.textViewCount);
            nameView = (TextView)v.findViewById(R.id.textViewName);
            numberView = (TextView)v.findViewById(R.id.textViewNumber);
        }
    }

    public AlexAdapter(List<AlexPhoneContact> list) { mList = list; }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.layout_alex,parent,false);
        final ViewHolder holder= new ViewHolder(view);
        return holder;
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        AlexPhoneContact phoneContact = mList.get(position);
        holder.countView.setText(String.valueOf(position+1));
        holder.nameView.setText(phoneContact.getDisplayName());
        holder.numberView.setText(phoneContact.getNumber());
    }

    @Override
    public int getItemCount() {
        return mList.size();
    }


}
