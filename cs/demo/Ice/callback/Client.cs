// **********************************************************************
//
// Copyright (c) 2003-2004 ZeroC, Inc. All rights reserved.
//
// This copy of Ice is licensed to you under the terms described in the
// ICE_LICENSE file included in this distribution.
//
// **********************************************************************


public class Client
{
    public static void Main(string[] args)
    {
        CallbackClient app = new CallbackClient();
        int status = app.main(args, "config");
        System.Environment.Exit(status);
    }
}
