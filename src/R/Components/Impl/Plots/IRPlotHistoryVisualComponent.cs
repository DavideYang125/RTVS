﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.R.Components.View;

namespace Microsoft.R.Components.Plots {
    public interface IRPlotHistoryVisualComponent : IVisualComponent {
        bool AutoHide { get; set; }
        IRPlot SelectedPlot { get; set; }
        bool CanDecreaseThumbnailSize { get; }
        bool CanIncreaseThumbnailSize { get; }
        void DecreaseThumbnailSize();
        void IncreaseThumbnailSize();
    }
}
