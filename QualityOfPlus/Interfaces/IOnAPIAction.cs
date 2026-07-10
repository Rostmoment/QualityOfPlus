using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.Interfaces
{
    /// <summary>
    /// Implement this on your <see cref="QOPFeature"/> to run code during <b>Quality Of Plus's own</b>
    /// loading sequence at the <see cref="MTM101BaldAPI.Registers.LoadingEventOrder.Pre"/> stage.<br/>
    /// This fires when QOP itself is loading, not your mod's own loading events —
    /// QOP calls <see cref="APIPreAction"/> automatically once your feature is registered,
    /// you don't call it yourself.
    /// </summary>
    public interface IOnAPIPre
    {
        /// <summary>
        /// Called by QOP during its own <see cref="MTM101BaldAPI.Registers.LoadingEventOrder.Pre"/> stage.<br/>
        /// Must yield return ones, otherwise game will softlock on loading screen<br/>
        /// QOP invokes this for you — do not call it manually.
        /// </summary>
        IEnumerator APIPreAction();
    }

    /// <summary>
    /// Implement this on your <see cref="QOPFeature"/> to run code during <b>Quality Of Plus's own</b>
    /// loading sequence at the <see cref="MTM101BaldAPI.Registers.LoadingEventOrder.Start"/> stage.<br/>
    /// This fires when QOP itself is loading, not your mod's own loading events —
    /// QOP calls <see cref="APIStartAction"/> automatically once your feature is registered,
    /// you don't call it yourself.
    /// </summary>
    public interface IOnAPIStart
    {
        /// <summary>
        /// Called by QOP during its own <see cref="MTM101BaldAPI.Registers.LoadingEventOrder.Start"/> stage.<br/>
        /// Must yield return ones, otherwise game will softlock on loading screen<br/>
        /// QOP invokes this for you — do not call it manually.
        /// </summary>
        IEnumerator APIStartAction();
    }

    /// <summary>
    /// Implement this on your <see cref="QOPFeature"/> to run code during <b>Quality Of Plus's own</b>
    /// loading sequence at the <see cref="MTM101BaldAPI.Registers.LoadingEventOrder.Post"/> stage.<br/>
    /// This fires when QOP itself is loading, not your mod's own loading events —
    /// QOP calls <see cref="APIPostAction"/> automatically once your feature is registered,
    /// you don't call it yourself.
    /// </summary>
    public interface IOnAPIPost
    {
        /// <summary>
        /// Called by QOP during its own <see cref="MTM101BaldAPI.Registers.LoadingEventOrder.Post"/> stage.<br/>
        /// Must yield return ones, otherwise game will softlock on loading screen<br/>
        /// QOP invokes this for you — do not call it manually.
        /// </summary>
        IEnumerator APIPostAction();
    }

    /// <summary>
    /// Implement this on your <see cref="QOPFeature"/> to run code during <b>Quality Of Plus's own</b>
    /// loading sequence at the <see cref="MTM101BaldAPI.Registers.LoadingEventOrder.Final"/> stage.<br/>
    /// This fires when QOP itself is loading, not your mod's own loading events —
    /// QOP calls <see cref="APIFinalAction"/> automatically once your feature is registered,
    /// you don't call it yourself.
    /// </summary>
    public interface IOnAPIFinal
    {
        /// <summary>
        /// Called by QOP during its own <see cref="MTM101BaldAPI.Registers.LoadingEventOrder.Final"/> stage.<br/>
        /// Must yield return ones, otherwise game will softlock on loading screen<br/>
        /// QOP invokes this for you — do not call it manually.
        /// </summary>
        IEnumerator APIFinalAction();
    }
}