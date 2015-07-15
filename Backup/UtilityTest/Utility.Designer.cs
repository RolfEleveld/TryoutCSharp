#pragma warning disable 0067, 0108
// ------------------------------------
// 
// Assembly Utility
// 
// ------------------------------------
namespace Utility.Stubs
{
    /// <summary>Stub of IFeedInterface</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.9.40105.0")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SFeedInterface
      : global::Microsoft.Stubs.StubBase<global::Utility.Stubs.SFeedInterface>
      , global::Utility.IFeedInterface
    {
        /// <summary>Initializes a new instance of type SFeedInterface</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SFeedInterface()
        {
        }

        /// <summary>Stub of method System.ServiceModel.Syndication.SyndicationFeedFormatter Utility.IFeedInterface.Feed(System.String format)</summary>
        public global::Microsoft.Stubs.StubDelegates.Func<global::Utility.Stubs.SFeedInterface, string, global::System.ServiceModel.Syndication.SyndicationFeedFormatter> Feed;

        /// <summary>Stub of method System.ServiceModel.Syndication.SyndicationFeedFormatter Utility.IFeedInterface.Feed(System.String format)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        global::System.ServiceModel.Syndication.SyndicationFeedFormatter global::Utility.IFeedInterface.Feed(string format)
        {
            global::Microsoft.Stubs
              .StubDelegates.Func<global::Utility.Stubs.SFeedInterface, 
              string, global::System.ServiceModel.Syndication
                .SyndicationFeedFormatter> sh = this.Feed;
            if ((object)sh != (object)null)
              return sh.Invoke(this, format);
            else 
            {
              global::Microsoft.Stubs.IDefaultStub<global::Utility.Stubs.SFeedInterface> 
                stub = 
                ((global::Microsoft.Stubs.IStub<global::Utility.Stubs.SFeedInterface>)this
                ).DefaultStub;
              return stub
                .Result<global::System.ServiceModel.Syndication.SyndicationFeedFormatter>
                  (this);
            }
        }
    }
}
namespace Utility.DynamiCloader.Stubs
{
    /// <summary>Stub of ILoadable</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.9.40105.0")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SLoadable
      : global::Microsoft.Stubs.StubBase<global::Utility.DynamiCloader.Stubs.SLoadable>
      , global::Utility.DynamiCloader.ILoadable
    {
        /// <summary>Initializes a new instance of type SLoadable</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SLoadable()
        {
        }

        /// <summary>Stub of method System.Void Utility.DynamiCloader.ILoadable.Execute()</summary>
        public global::Microsoft.Stubs.StubDelegates.Action<global::Utility.DynamiCloader.Stubs.SLoadable> Execute;

        /// <summary>Stub of method System.Void Utility.DynamiCloader.ILoadable.Execute()</summary>
        [global::System.Diagnostics.DebuggerHidden]
        void global::Utility.DynamiCloader.ILoadable.Execute()
        {
            global::Microsoft.Stubs
              .StubDelegates.Action<global::Utility.DynamiCloader.Stubs.SLoadable> sh
               = this.Execute;
            if ((object)sh != (object)null)
              sh.Invoke(this);
            else 
            {
              global::Microsoft.Stubs
                .IDefaultStub<global::Utility.DynamiCloader.Stubs.SLoadable> stub
                 = ((global::Microsoft.Stubs
                  .IStub<global::Utility.DynamiCloader.Stubs.SLoadable>)this).DefaultStub;
              stub.VoidResult(this);
            }
        }
    }
}

