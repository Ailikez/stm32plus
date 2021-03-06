﻿/*
 * This file is shared between the open source stm32plus and
 * Arduino XMEM graphics libraries.
 * 
 * Copyright (c) 2011,2012 Andy Brown <www.andybrown.me.uk>
 * Please see website for licensing terms.
 */

using System;
using System.IO;
using System.Windows.Forms;

namespace LzgFontConv {

  /// <summary>
  /// specialisations for writing Arduino font files
  /// </summary>
  
  public class ArduinoFontWriter : FontWriter {

    /// <summary>
    /// write the header start 
    /// </summary>

    protected override void WriteHeaderStart(TextWriter writer) {
      
      writer.Write("#pragma once\n\n");
      writer.Write("#include \"Font.h\"\n\n");
      writer.Write("namespace lcd {\n\n");
    }


    /// <summary>
    /// write the header end
    /// </summary>

    protected override void WriteHeaderEnd(TextWriter writer) {
      writer.Write("}\n");
    }


    /// <summary>
    /// write the source start
    /// </summary>

    protected override void WriteSourceStart(TextWriter writer) {
      
      writer.Write("#include <avr/pgmspace.h>\n");
      writer.Write("#include \"Font.h\"\n\n");
      writer.Write("namespace lcd {\n\n");
    }


    /// <summary>
    /// write the source end
    /// </summary>

    protected override void WriteSourceEnd(TextWriter writer) {
      writer.Write("}\n");
    }


    /// <summary>
    /// Write the source body
    /// </summary>

    protected override void WriteSourceBody(TextWriter writer) {

      bool first;

      writer.Write("  // byte definitions for "+GetFontNameAndSize()+"\n\n");

      foreach(CharDef cd in _charDefs.Definitions) {

        // don't write empty arrays (space character)

        if(cd.CompressedBytes==null || cd.CompressedBytes.Length==0)
          continue;

        // opening declaration

        writer.Write("  const uint8_t __attribute__((progmem)) "+GetCharacterName(cd.Character)+"[] PROGMEM={ ");

        // bytes

        first=true;
        foreach(byte b in cd.CompressedBytes) {
          
          if(first)
            first=false;
          else
            writer.Write(",");
          
          writer.Write(b.ToString());
        }

        // closing declaration

        writer.Write("};\n");
      }

      writer.Write("\n  // character definitions for "+GetFontNameAndSize()+"\n\n");
      writer.Write("  extern const struct FontChar __attribute__((progmem)) FDEF_"+GetFontName()+"_CHAR[] PROGMEM={\n");

      foreach(CharDef cd in _charDefs.Definitions) {
        
        // don't write empty arrays (space character)

        writer.Write("    { ");

        writer.Write(Convert.ToUInt16(cd.Character).ToString()+",");
        writer.Write(cd.Size.Width+",");
        
        writer.Write((cd.Character==' ' ? "(const uint8_t *)0" : GetCharacterName(cd.Character))+" },\n");
      }

      writer.Write("  };\n\n");
    }
  }
}
