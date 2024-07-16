mergeInto(LibraryManager.library, {
	RecibirData: function (str) {
		window.receiveMessageFromUnity(Pointer_stringify(str));
	}
});