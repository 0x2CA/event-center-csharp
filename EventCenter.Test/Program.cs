using System;
using static EventCenter;

class Program {

    class TestEvent : IEvent {
        public readonly int Code;
        public readonly string Message;
        public TestEvent (int code, string message) {
            Code = code;
            Message = message;
        }
    }

    static EventCenter _EventCenter = new EventCenter ();
    static void Main (string[] args) {
        Action<TestEvent> typeCallback = TypeCallBack;
        _EventCenter.On (typeCallback);

        _EventCenter.Emit (new TestEvent (10, "Test"));

        _EventCenter.Emit (new TestEvent (10, "Test"));

        _EventCenter.Off (typeCallback);

        _EventCenter.Emit (new TestEvent (10, "Test"));

        _EventCenter.Once (typeCallback);

        _EventCenter.Emit (new TestEvent (10, "Test"));

        _EventCenter.Emit (new TestEvent (10, "Test"));

        // string event

        _EventCenter.On ("test", StingCallBack);

        _EventCenter.Emit ("test", "123", 798, "321");

        _EventCenter.Emit ("test", "123", 798, "321");

        _EventCenter.Off ("test", StingCallBack);

        _EventCenter.Emit ("test", "123", 798, "321");

        _EventCenter.Once ("test", StingCallBack);

        _EventCenter.Emit ("test", "123", 798, "321");

        _EventCenter.Emit ("test", "123", 798, "321");

        // Number Event

        _EventCenter.On (1, StingCallBack);

        _EventCenter.Emit (1, "000", 123, "000");

        _EventCenter.Emit (1, "000", 123, "000");

        _EventCenter.Off (1, StingCallBack);

        _EventCenter.Emit (1, "000", 123, "000");

        _EventCenter.Once (1, StingCallBack);

        _EventCenter.Emit (1, "000", 123, "000");

        _EventCenter.Emit (1, "000", 123, "000");
    }

    static void TypeCallBack (TestEvent args) {
        Console.WriteLine ("Code " + args.Code + " Message " + args.Message);

    }

    static void StingCallBack (params object[] args) {
        Console.WriteLine (args[0] + " " + args[1] + " " + args[2]);
    }

}