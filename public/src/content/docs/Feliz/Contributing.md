---
title: Contributing
---

If you are enjoying the Feliz project, consider contributing back and helping out with the community. There are many ways you can contribute, the first of which is to simply use Feliz in your project and report issues you come across whether there are missing style properties or properties that were incorrectly implemeneted.

### More documentation and samples
You can also contribute to the documentation of the Feliz and it's ecosystem: creating sample code with live examples is really appreciated as it helps test out the library and creates a greate reference for using it. In the [Feliz.Recharts](#/Recharts/Overiew) library for example, you can help out translating many examples from the original documentation into live samples for reference and for testing the library like we are doing here.


## Writing Docs

You can create live example for feliz components by writing adding a `.fs` file to `src/components/fsharp/Docs.fsproj`. You can then use your new component as such in any `.mdx` file:

```mdx
import { Code } from '@astrojs/starlight/components';
import rawCounter from '/src/components/fsharp/Counter.fs?raw';
import {Counter} from '/src/components/fsharp/Counter.jsx';

<Code code={rawCounter} lang='fsharp'/>
<Counter client:only="react"/>
```