// **********************************************************************
//
// Copyright (c) 2003-2004 ZeroC, Inc. All rights reserved.
//
// This copy of Ice is licensed to you under the terms described in the
// ICE_LICENSE file included in this distribution.
//
// **********************************************************************


public class Server
{
    private static void usage()
    {
        System.Console.Error.WriteLine("Usage: Server port");
    }
    
    private static int run(string[] args, Ice.Communicator communicator)
    {
        int port = 0;
        for(int i = 0; i < args.Length; i++)
        {
            if(args[i][0] == '-')
            {
                System.Console.Error.WriteLine("Server: unknown option `" + args[i] + "'");
                usage();
                return 1;
            }
            
            if(port != 0)
            {
                System.Console.Error.WriteLine("Server: only one port can be specified");
                usage();
                return 1;
            }
            
            try
            {
                port = System.Int32.Parse(args[i]);
            }
            catch(System.FormatException)
            {
                System.Console.Error.WriteLine("Server: invalid port");
                usage();
                return 1;
            }
        }
        
        if(port <= 0)
        {
            System.Console.Error.WriteLine("Server: no port specified");
            usage();
            return 1;
        }
        
        communicator.getProperties().setProperty("TestAdapter.Endpoints", "default -p " + port);
        Ice.ObjectAdapter adapter = communicator.createObjectAdapter("TestAdapter");
        Ice.Object obj = new TestI(adapter);
        adapter.add(obj, Ice.Util.stringToIdentity("test"));
        adapter.activate();
        communicator.waitForShutdown();
        return 0;
    }
    
    public static void Main(string[] args)
    {
        int status = 0;
        Ice.Communicator communicator = null;
        
        try
        {
            communicator = Ice.Util.initialize(ref args);
            status = run(args, communicator);
        }
        catch(System.Exception ex)
        {
	    System.Console.Error.WriteLine(ex);
            status = 1;
        }
        
        if(communicator != null)
        {
            try
            {
                communicator.destroy();
            }
            catch(Ice.LocalException ex)
            {
		System.Console.Error.WriteLine(ex);
                status = 1;
            }
        }
        
        System.Environment.Exit(status);
    }
}
