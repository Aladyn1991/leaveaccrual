﻿// See https://aka.ms/new-console-template for more information

using here;
using nobody.console;
using nobody.core;
using nobody.online;

var nobody = new Nobody(); // find a way to avoid this

nobody.online()
    .visit("https://cbs.almansoori.biz")
    .fill("username".get_string(), "#logonuidfield")
    .fill("password".get_string(), "#logonpassfield")
    .click("input[type='submit']")
    .show("info", "Logged in")
    .wait_for_navigation();


//test console logging
 nobody.show("Hello", "there...")
    .show("info", "Hello", "there...")
    .show("warning", "Hello", "there...")
    .show("error", "Hello", "there...")
    .show("this", "is", "a", "test", "of", "the", "emergency", "broadcast", "system")
    .ask("name", "What is your name?")
    .ask_number("age")
    .ask_secret("password")
    .ask_choice("color", new[] { "red", "green", "blue" });
    // .online_async();
//test object logging
nobody.show(new TestClass{Name="John",Age=42});

nobody.show((ctx)=> {
    ctx["name"] = "John";
    ctx["age"] = 42;
    return "Hello" + ctx["name"] + "you are " + ctx["age"] + "years old";
});