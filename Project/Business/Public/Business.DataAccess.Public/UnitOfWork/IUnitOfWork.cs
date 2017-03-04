﻿using System;
using Business.DataAccess.Public.Repository;

namespace Business.DataAccess.Public.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IProfileRepository Profiles { get; }
        IProfileActionRepository ProfileActions { get; }
        IProfileActionLikeRepository ProfileActionLikes { get; }


        void Complete();
    }
}
