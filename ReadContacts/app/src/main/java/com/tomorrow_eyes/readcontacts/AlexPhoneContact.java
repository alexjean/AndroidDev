package com.tomorrow_eyes.readcontacts;

public class AlexPhoneContact {
    private String DisplayName;
    private String Number;

    AlexPhoneContact(String name,String number) {
        this.DisplayName= name;
        this.Number = number;
    }

    public String getDisplayName() {
        return DisplayName;
    }

    public String getNumber() {
        return Number;
    }
}
