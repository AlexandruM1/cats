﻿CREATE TABLE [Logistics].[HubAllocation] (
    [HubAllocationID] INT      IDENTITY (1, 1) NOT NULL,
    [RequisitionID]   INT      NOT NULL,
    [HubID]           INT      NOT NULL,
    [AllocationDate]  DATETIME NOT NULL,
    [AllocatedBy]     INT      NOT NULL,
    CONSTRAINT [PK_HubAllocation] PRIMARY KEY CLUSTERED ([HubAllocationID] ASC),
    CONSTRAINT [FK_HubAllocation_Hub] FOREIGN KEY ([HubID]) REFERENCES [dbo].[Hub] ([HubID]),
    CONSTRAINT [FK_HubAllocation_UserProfile] FOREIGN KEY ([AllocatedBy]) REFERENCES [dbo].[UserProfile] ([UserProfileID])
);



