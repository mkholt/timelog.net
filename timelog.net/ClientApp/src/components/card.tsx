import { Stack, Text, getTheme, mergeStyles } from "@fluentui/react"
import React from "react"

export type ICardProps = {
	title: string,
	children: JSX.Element
}

let theme = getTheme()
let styles = mergeStyles({
	boxShadow: theme.effects.elevation4,
	borderTop: '5px solid ' + theme.palette.accent,
	padding: 5,
	background: '#FFF'
})

export const Card = ({ title, children }: ICardProps) => (
	<Stack className={styles}>
		<Text variant="large">{title}</Text>
		{children}
	</Stack>
)