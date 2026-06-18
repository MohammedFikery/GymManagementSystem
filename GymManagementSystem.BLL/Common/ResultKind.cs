using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Common
{
    public enum ResultKind
    {
        OK = 0,                 // 200 / 201
        ValidationFailed = 1,   // 400
        NotFound = 2,           // 404
        Conflict = 3,           // 409
        BusinessRule = 4,       // 422
        Unauthorized = 5,       // 401
        Forbidden = 6,          // 403
        Failure = 7             // 500
    }
}
