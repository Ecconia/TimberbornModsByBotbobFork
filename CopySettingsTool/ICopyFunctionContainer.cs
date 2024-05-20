using System.Collections.Generic;
using Timberborn.BlockSystem;

namespace CopySettingsTool
{
  public interface ICopyFunctionContainer
  {
    string Label { get; }

    //List<BlockObject> CopyableComponents { get; }

    /*
    bool PreCopyCheck(BlockObject savedObject, BlockObject blockObject);
    */

    void CopySettings(BlockObject savedObject, BlockObject blockObject);

    /*
    void PostCopyCall(BlockObject savedObject, BlockObject blockObject);
    */
  }
}