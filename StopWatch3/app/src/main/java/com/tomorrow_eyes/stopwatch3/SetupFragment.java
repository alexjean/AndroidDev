package com.tomorrow_eyes.stopwatch3;

import android.app.Activity;
import android.content.Context;
import android.os.Bundle;
import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.tomorrow_eyes.stopwatch3.databinding.FragmentSetupBinding;


/**
 * A simple {@link Fragment} subclass.
 * Use the {@link SetupFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class SetupFragment extends Fragment {

    public interface SetSeconds {
        void setSeconds(int seconds);
    }


    private static final String strSeconds = "Seconds";

    private int iSeconds;
    private Activity mActivity;
    SetSeconds mSetSeconds;

    private FragmentSetupBinding binding;

    public SetupFragment() {
        // Required empty public constructor
    }

    @Override
    public void onActivityCreated(@Nullable Bundle savedInstanceState) {
        super.onActivityCreated(savedInstanceState);
        // Button btn= mActivity.findViewById(R.id.btnSetup);
        // btn.setOnClickListener(new View.OnClickListener() {
        //    @Override
        //    public void onClick(View view) {
        //        EditText editText = mActivity.findViewById(R.id.editSeconds);
        //        String str = editText.getText().toString();

        binding.btnSetup.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                String str = binding.editSeconds.getText().toString();
                if (mSetSeconds != null) {
                    iSeconds = Integer.parseInt(str);
                    mSetSeconds.setSeconds(iSeconds);
                    mActivity.onBackPressed();
                }
            }
        });
    }

    public static SetupFragment newInstance(Integer seconds) {
        SetupFragment fragment = new SetupFragment();
        Bundle args = new Bundle();
        args.putInt(strSeconds, seconds);
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public void onAttach(Context context) {
        super.onAttach(context);
        mActivity = (Activity) context;
        if (context instanceof  SetSeconds)
            mSetSeconds = (SetSeconds) context;
        if (getArguments() != null )
            iSeconds = getArguments().getInt(strSeconds);
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (getArguments() != null) {
            iSeconds = getArguments().getInt(strSeconds);
        }
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        // View root = inflater.inflate(R.layout.fragment_setup, container, false);
        // EditText editSeconds = root.findViewById(R.id.editSeconds);
        // editSeconds.setText(Integer.toString(iSeconds));
        // return root;

        // 用 binding不用再 findViewById(R...)
        binding = FragmentSetupBinding.inflate(inflater);
        binding.editSeconds.setText(Integer.toString(iSeconds));
        return binding.getRoot();
    }
}
