using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ContextListener
{
    ContextManager.ContextChangedHandler ContextChangedListener { get; set; }
}
