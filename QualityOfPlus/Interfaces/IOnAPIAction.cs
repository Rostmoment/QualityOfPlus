using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace QualityOfPlus.Interfaces
{
    public interface IOnAPIPre
    {
        /// <summary>
        /// Action to execute in <see cref="MTM101BaldAPI.Registers.LoadingEventOrder.Pre"/><br>
        /// Everything must be done in one yield keyword, otherwise game will softlock on loading screen<br>
        /// QOP will execute action itself, no need to call it manually
        /// </summary>
        IEnumerator Action();
    }

    public interface IOnAPIStart
    {
        /// <summary>
        /// Action to execute in <see cref="MTM101BaldAPI.Registers.LoadingEventOrder.Start"/><br>
        /// Everything must be done in one yield keyword, otherwise game will softlock on loading screen<br>
        /// QOP will execute action itself, no need to call it manually
        /// </summary>
        IEnumerator Action();
    }

    public interface IOnAPIPost
    {
        /// <summary>
        /// Action to execute in <see cref="MTM101BaldAPI.Registers.LoadingEventOrder.Post"/><br>
        /// Everything must be done in one yield keyword, otherwise game will softlock on loading screen<br>
        /// QOP will execute action itself, no need to call it manually
        /// </summary>
        IEnumerator Action();
    }
    public interface IOnAPIFinal
    {
        /// <summary>
        /// Action to execute in <see cref="MTM101BaldAPI.Registers.LoadingEventOrder.Final"/><br>
        /// Everything must be done in one yield keyword, otherwise game will softlock on loading screen<br>
        /// QOP will execute action itself, no need to call it manually
        /// </summary>
        IEnumerator Action();
    }
}
