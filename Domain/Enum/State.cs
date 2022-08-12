// <copyright file="State.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Domain.Enum
{
    using System;

    /// <summary>
    /// Course progress state.
    /// </summary>
    [Serializable]
    public enum State
    {
        NotCompleted = 0,
        Completed = 1
    }
}
