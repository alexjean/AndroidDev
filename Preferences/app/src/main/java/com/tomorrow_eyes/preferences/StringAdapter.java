package com.tomorrow_eyes.preferences;

import android.support.annotation.NonNull;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import java.util.List;

public class StringAdapter extends RecyclerView.Adapter<StringAdapter.ViewHolder> {

    private List<String> mList;

    static class ViewHolder extends RecyclerView.ViewHolder {
        View stringView;
        TextView msgView;
        private ViewHolder(View itemView) {
            super(itemView);
            stringView = itemView;
            msgView = (TextView)itemView.findViewById(R.id.msgLine);
        }
    }

    public StringAdapter(List<String> list ) { mList = list; }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.string_layout,parent, false);
        final ViewHolder holder = new ViewHolder(view);
        return holder;
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        String line = mList.get(position);
        holder.msgView.setText(line);
    }

    @Override
    public int getItemCount() {
        return mList.size();
    }
}
