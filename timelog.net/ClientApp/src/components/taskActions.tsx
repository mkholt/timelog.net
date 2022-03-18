import { ActionButton, Stack } from "@fluentui/react"
import React from "react"
import { buttonStyles } from "./actions"
import { ITask } from "./pages/project"
import { useNavigate } from "react-router-dom"

export type ActionProps = {
	item: ITask
}

export const TaskActions = ({ item }: ActionProps) => {
	const navigate = useNavigate()
	const onEdit = React.useCallback(() => navigate("task/" + item.taskId), [item, navigate])
	const onDelete = React.useCallback(() => alert('Deleting ' + item.taskId), [item])

	return (
		<Stack horizontal>
			<ActionButton title="Edit" iconProps={{ iconName: 'Edit' }} styles={buttonStyles} onClick={onEdit} />
			<ActionButton title="Delete" iconProps={{ iconName: 'Delete' }} styles={buttonStyles} onClick={onDelete} />
			{item.url && (
				<ActionButton
					title="Open externally"
					iconProps={{ iconName: item.icon }}
					styles={buttonStyles} href={item.url}
					target="_blank"
				/>)}
		</Stack>
	)
}
