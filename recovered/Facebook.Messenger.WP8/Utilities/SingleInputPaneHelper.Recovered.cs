// Ricostruzione manuale dal IL di Facebook.Messenger.WP8.Utilities.SingleInputPaneHelper.
//
// ILSpy 11.2.0.10050 fallisce su questi due metodi con
//   ArgumentNullException: Value cannot be null. (Parameter 'source')
//   in ICSharpCode.Decompiler.CSharp.Disambiguator (via Enumerable.Take)
// cioè dentro la risoluzione degli argomenti di WindowsRuntimeMarshal.AddEventHandler,
// non nel codice dell'app. Il IL è completo e leggibile: qui sotto è tradotto a mano.
//
// File originale (dai ldstr di log): c:\Open\messenger\Facebook\Facebook-Messenger-WP8\Utilities\SingleInputPaneHelper.cs

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.ViewManagement;

namespace Facebook.Messenger.WP8.Utilities
{
    public class SingleInputPaneHelper
    {
        private InputPaneHidingHandler _inputPaneHidingHandler;
        private InputPaneShowingHandler _inputPaneShowingHandler;

        // IL_0000  call InputPane::GetForCurrentView() -> stloc.0
        // IL_0007..IL_002a  WindowsRuntimeMarshal.AddEventHandler<...>(add_Showing, remove_Showing, ShowingHandler)
        // IL_0030..IL_0053  WindowsRuntimeMarshal.AddEventHandler<...>(add_Hiding,  remove_Hiding,  HidingHandler)
        // IL_0058..IL_0061  AddShowingHandler(arg.1) + AddHidingHandler(arg.2)
        public void SubscribeToKeyboardEvents(InputPaneShowingHandler showingHandler, InputPaneHidingHandler hidingHandler)
        {
            InputPane inputPane = InputPane.GetForCurrentView();
            inputPane.Showing += ShowingHandler;
            inputPane.Hiding += HidingHandler;
            AddShowingHandler(showingHandler);
            AddHidingHandler(hidingHandler);
        }

        // IL_0007..IL_001e  WindowsRuntimeMarshal.RemoveEventHandler<...>(remove_Showing, ShowingHandler)
        // IL_0024..IL_003b  WindowsRuntimeMarshal.RemoveEventHandler<...>(remove_Hiding,  HidingHandler)
        // IL_0040..IL_0049  i due campi delegato vengono azzerati
        public void UnsubscribeFromKeyboardEvents()
        {
            InputPane inputPane = InputPane.GetForCurrentView();
            inputPane.Showing -= ShowingHandler;
            inputPane.Hiding -= HidingHandler;
            _inputPaneShowingHandler = null;
            _inputPaneHidingHandler = null;
        }

        // IL_0001..IL_0006  brfalse su _inputPaneShowingHandler
        // IL_0008..IL_001f  Logger.Warning(this, "A handler is already registered!", ..., riga 38) + ret
        // IL_0020..IL_0022  altrimenti assegna il campo
        private void AddShowingHandler(InputPaneShowingHandler handler)
        {
            if (_inputPaneShowingHandler != null)
            {
                Facebook.Base.Logging.Logger.Warning(
                    this,
                    "A handler is already registered!",
                    "AddShowingHandler",
                    "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\SingleInputPaneHelper.cs",
                    38);
                return;
            }

            _inputPaneShowingHandler = handler;
        }

        // Simmetrico al precedente, log alla riga 50.
        private void AddHidingHandler(InputPaneHidingHandler handler)
        {
            if (_inputPaneHidingHandler != null)
            {
                Facebook.Base.Logging.Logger.Warning(
                    this,
                    "A handler is already registered!",
                    "AddHidingHandler",
                    "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\SingleInputPaneHelper.cs",
                    50);
                return;
            }

            _inputPaneHidingHandler = handler;
        }

        // IL_0001..IL_0008  se il campo è null -> ret (nessuna proprietà toccata)
        // IL_0009..IL_000b  e.EnsuredFocusedElementInView = true (put_EnsuredFocusedElementInView)
        // IL_0010..IL_0018  callvirt Invoke(object, InputPaneVisibilityEventArgs)
        private void ShowingHandler(InputPane sender, InputPaneVisibilityEventArgs e)
        {
            if (_inputPaneShowingHandler == null)
            {
                return;
            }

            e.EnsuredFocusedElementInView = true;
            _inputPaneShowingHandler(sender, e);
        }

        // Identico a ShowingHandler ma con InputPaneHidingHandler: il delegato
        // accetta un InputPane, quindi non c'è boxing verso object.
        private void HidingHandler(InputPane sender, InputPaneVisibilityEventArgs e)
        {
            if (_inputPaneHidingHandler == null)
            {
                return;
            }

            e.EnsuredFocusedElementInView = true;
            _inputPaneHidingHandler(sender, e);
        }
    }

    // I delegati sono in due file separati dell'assembly: recuperati senza errori,
    // qui solo per rendere leggibile la firma usata sopra.
    public delegate void InputPaneShowingHandler(object sender, InputPaneVisibilityEventArgs e);

    public delegate void InputPaneHidingHandler(InputPane input, InputPaneVisibilityEventArgs e);
}
