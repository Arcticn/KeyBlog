import { config } from 'md-editor-v3';
import anchor from 'markdown-it-anchor';

config({
  markdownItConfig(md) {
    md.use(anchor, {
      permalink: anchor.permalink.headerLink({ safariReaderFix: true }),
    })
  }
});