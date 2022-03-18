import React from 'react';
import { Stack, Text, Link, FontWeights, IStackTokens } from '@fluentui/react';

const boldStyle = { root: { fontWeight: FontWeights.semibold } };
const stackTokens: IStackTokens = { childrenGap: 15 };

export const Topbar = () => (
	<Stack horizontal tokens={stackTokens}>
        <Stack.Item grow>
          <Text variant="xxLargePlus" styles={boldStyle}>Time Logger</Text>
        </Stack.Item>
        <Link href="https://developer.microsoft.com/en-us/fluentui#/get-started/web">Docs</Link>
        <Link href="https://stackoverflow.com/questions/tagged/office-ui-fabric">Stack Overflow</Link>
        <Link href="https://github.com/mkholt/timelog.net">Github</Link>
        <Link href="https://twitter.com/fluentui">Twitter</Link>
      </Stack>
)