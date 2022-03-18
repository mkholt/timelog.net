import { ActionButton, IButtonStyles, Stack } from "@fluentui/react"
import React from "react"
import { IItem } from "./pages/project"

export const buttonStyles: IButtonStyles = {
	root: { height: 16 },
	icon: { margin: 0 }
}

export type ActionProps = {
	item: IItem
}

export const Actions = ({ item }: ActionProps) => {
	const onEdit = React.useCallback(() => alert('Editing ' + item.id), [item])
	const onDelete = React.useCallback(() => alert('Deleting ' + item.id), [item])

	return (
		<Stack horizontal style={{ textAlign: 'right' }}>
			<Stack.Item grow>&nbsp;</Stack.Item>
			<ActionButton title="Edit" iconProps={{ iconName: 'Edit' }} styles={buttonStyles} onClick={onEdit} />
			<ActionButton title="Delete" iconProps={{ iconName: 'Delete' }} styles={buttonStyles} onClick={onDelete} />
		</Stack>
	)
}