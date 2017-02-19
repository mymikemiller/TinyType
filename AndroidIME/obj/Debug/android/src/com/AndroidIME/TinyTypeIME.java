package com.AndroidIME;


public class TinyTypeIME
	extends android.inputmethodservice.InputMethodService
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:()V:GetOnCreateHandler\n" +
			"n_onWindowShown:()V:GetOnWindowShownHandler\n" +
			"n_onCreateCandidatesView:()Landroid/view/View;:GetOnCreateCandidatesViewHandler\n" +
			"n_onCreateInputView:()Landroid/view/View;:GetOnCreateInputViewHandler\n" +
			"";
		mono.android.Runtime.register ("AndroidIME.TinyTypeIME, AndroidIME, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", TinyTypeIME.class, __md_methods);
	}


	public TinyTypeIME () throws java.lang.Throwable
	{
		super ();
		if (getClass () == TinyTypeIME.class)
			mono.android.TypeManager.Activate ("AndroidIME.TinyTypeIME, AndroidIME, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate ()
	{
		n_onCreate ();
	}

	private native void n_onCreate ();


	public void onWindowShown ()
	{
		n_onWindowShown ();
	}

	private native void n_onWindowShown ();


	public android.view.View onCreateCandidatesView ()
	{
		return n_onCreateCandidatesView ();
	}

	private native android.view.View n_onCreateCandidatesView ();


	public android.view.View onCreateInputView ()
	{
		return n_onCreateInputView ();
	}

	private native android.view.View n_onCreateInputView ();

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
