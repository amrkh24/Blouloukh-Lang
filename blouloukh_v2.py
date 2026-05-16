#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
██████╗ ██╗      ██████╗ ██╗   ██╗██╗      ██████╗ ██╗  ██╗██╗  ██╗
██╔══██╗██║     ██╔═══██╗██║   ██║██║     ██╔═══██╗██║ ██╔╝██║  ██║
██████╔╝██║     ██║   ██║██║   ██║██║     ██║   ██║█████╔╝ ███████║
██╔══██╗██║     ██║   ██║██║   ██║██║     ██║   ██║██╔═██╗ ██╔══██║
██████╔╝███████╗╚██████╔╝╚██████╔╝███████╗╚██████╔╝██║  ██╗██║  ██║
╚═════╝ ╚══════╝ ╚═════╝  ╚═════╝ ╚══════╝ ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝

لغة البرمجة البلولوخ - إصدار الشباب "حاسب يا أسطى" v0.2
"""

import re
import sys

# ─────────────────────────────────────────────
#  Tokenizer
# ─────────────────────────────────────────────

KEYWORDS = [
    'طب افرض', 'وعلى وضعك', 'مش يساوي', 'سكّة تانية',
    'حاسب', 'لفّ وارجع تاني', 'اركن على جنب', 'كمّل',
    'سمّعنا', 'دوّر المحرك', 'هات اللي معاك',
    'ثبّت حضور', 'ثابت', 'انزل بالجديد', 'خلّص الكلام',
    'يساوي', 'و', 'او', 'مش', 'صح', 'غلط', 'فاضي'
]

def tokenize(code):
    tokens = []
    i = 0
    line_num = 1
    kw_pattern = '|'.join(re.escape(k) for k in sorted(KEYWORDS, key=len, reverse=True))

    while i < len(code):
        if code[i] in ' \t':
            i += 1
            continue
        if code[i] == '\n':
            line_num += 1
            i += 1
            continue
        if code[i:i+2] == '//':
            end = code.find('\n', i)
            i = end if end != -1 else len(code)
            continue
        if code[i] == '"':
            j = i + 1
            while j < len(code) and code[j] != '"':
                j += 1
            tokens.append(('STRING', code[i+1:j], line_num))
            i = j + 1
            continue
        m = re.match(r'\d+(\.\d+)?', code[i:])
        if m:
            val = m.group()
            tokens.append(('NUMBER', float(val) if '.' in val else int(val), line_num))
            i += len(val)
            continue
        m = re.match(kw_pattern, code[i:])
        if m:
            kw = m.group()
            tokens.append(('KW', kw, line_num))
            i += len(kw)
            continue
        m = re.match(r'[\w\u0600-\u06FF]+', code[i:])
        if m:
            tokens.append(('ID', m.group(), line_num))
            i += len(m.group())
            continue
        if code[i] in '+-*/%<>=!(),:[]':
            tokens.append(('OP', code[i], line_num))
            i += 1
        else:
            i += 1
    return tokens

if __name__ == '__main__':
    print("🔥 البلولوخ v0.2 - حاسب يا أسطى 🔥")
    if len(sys.argv) > 1:
        print(f"جاري تشغيل: {sys.argv[1]}")
    else:
        print("REPL جاهز... اكتب 'خروج' للقفل")
