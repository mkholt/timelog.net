import React from 'react';
import { Stack } from "@fluentui/react/lib/Stack"
import {Menubar} from "./menubar";
import {Topbar} from "./topbar";
import { getTheme } from "@fluentui/react"

type LayoutProps = {}

const theme = getTheme()

export const Layout = (props: React.PropsWithChildren<LayoutProps>) => {
    return (
        <Stack tokens={{ padding: '0.5em 1em 0 1em', childrenGap: theme.spacing.m }}>
            <Topbar />
            <Stack horizontal tokens={{ childrenGap: theme.spacing.s2 }}>
                <Menubar />
                <Stack.Item grow disableShrink>
                    {props.children}
                </Stack.Item>
            </Stack>
        </Stack>
    )
}